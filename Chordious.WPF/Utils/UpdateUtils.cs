// 
// UpdateUtils.cs
//  
// Author:
//       Jon Thysell <thysell@gmail.com>
// 
// Copyright (c) 2015, 2016, 2017 Jon Thysell <http://jonthysell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml;

using GalaSoft.MvvmLight.Messaging;

using com.jonthysell.Chordious.Core.ViewModel;

using com.jonthysell.Chordious.WPF.Resources;

namespace com.jonthysell.Chordious.WPF
{
    public class UpdateUtils
    {
        public static AppViewModel AppVM
        {
            get
            {
                return AppViewModel.Instance;
            }
        }

        public static bool IsCheckingforUpdate { get; private set; }

        public static Task UpdateCheckAsync(bool confirmUpdate, bool showUpToDate)
        {
            return Task.Factory.StartNew(() =>
            {
                UpdateCheck(confirmUpdate, showUpToDate);
            });
        }

        public static void UpdateCheck(bool confirmUpdate, bool showUpToDate)
        {
            try
            {
                IsCheckingforUpdate = true;

                List<InstallerInfo> installerInfos = GetLatestInstallerInfos();

                ReleaseChannel targetReleaseChannel = GetReleaseChannel();

                ulong maxVersion = LongVersion(AppVM.FullVersion);

                InstallerInfo latestVersion = null;

                bool updateAvailable = false;
                foreach (InstallerInfo installerInfo in installerInfos)
                {
                    if (installerInfo.ReleaseChannel == targetReleaseChannel)
                    {
                        ulong installerVersion = LongVersion(installerInfo.Version);

                        if (installerVersion > maxVersion)
                        {
                            updateAvailable = true;
                            latestVersion = installerInfo;
                            maxVersion = installerVersion;
                        }
                    }
                }

                LastUpdateCheck = DateTime.Now;

                if (updateAvailable)
                {
                    if (confirmUpdate)
                    {
                        string message = String.Format(Strings.ChordiousUpdateAvailableUpdateNowMessageFormat, latestVersion.Version);
                        AppVM.DoOnUIThread(() =>
                        {
                            Messenger.Default.Send<ConfirmationMessage>(new ConfirmationMessage(message, (confirmed) =>
                            {
                                if (confirmed)
                                {
                                    Update(latestVersion);
                                }
                            }));
                        });
                    }
                    else
                    {
                        Update(latestVersion);
                    }
                }
                else
                {
                    if (showUpToDate)
                    {
                        AppVM.DoOnUIThread(() =>
                        {
                            Messenger.Default.Send<ChordiousMessage>(new ChordiousMessage(Strings.ChordiousUpdateNotAvailableMessage));
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(new UpdateException(ex));
            }
            finally
            {
                IsCheckingforUpdate = false;
            }
        }

        private static void Update(InstallerInfo installerInfo)
        {
            if (null == installerInfo)
            {
                throw new ArgumentNullException("installerInfo");
            }

            if (!UpdateUtils.IsConnectedToInternet)
            {
                throw new UpdateNoInternetException();
            }

            string tempPath = Path.GetTempPath();

            string msiPath = Path.Combine(tempPath, "ChordiousSetup.msi");

            if (File.Exists(msiPath))
            {
                File.Delete(msiPath);
            }

            using (WebClient client = new WebClient())
            {
                client.Headers["User-Agent"] = _userAgent;
                client.DownloadFile(installerInfo.Url, msiPath);
            }

            string cmdFile = Path.Combine(tempPath, "UpdateChordious.cmd");

            using (StreamWriter sw = new StreamWriter(new FileStream(cmdFile, FileMode.Create)))
            {
                sw.WriteLine("msiexec /i \"{0}\" /qb", msiPath);
            }

            AppVM.DoOnUIThread(() =>
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("cmd.exe", String.Format("/c {0}", cmdFile));
                p.StartInfo.CreateNoWindow = true;
                p.Start();

                App.Current.Shutdown();
            });
        }

        public static List<InstallerInfo> GetLatestInstallerInfos()
        {
            if (!UpdateUtils.IsConnectedToInternet)
            {
                throw new UpdateNoInternetException();
            }

            List<InstallerInfo> installerInfos = new List<InstallerInfo>();

            HttpWebRequest request = WebRequest.CreateHttp(_updateUrl);
            request.UserAgent = _userAgent;

            using (XmlReader reader = XmlReader.Create(request.GetResponse().GetResponseStream()))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "update")
                        {
                            string version = reader.GetAttribute("version");
                            string url = reader.GetAttribute("url");
                            ReleaseChannel releaseChannel = (ReleaseChannel)Enum.Parse(typeof(ReleaseChannel), reader.GetAttribute("channel"));
                            installerInfos.Add(new InstallerInfo(version, url, releaseChannel));
                        }
                    }
                }
            }

            return installerInfos;
        }

        public static ulong LongVersion(string version)
        {
            if (String.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException("version");
            }

            ulong vers = 0;

            string[] parts = version.Trim().Split('.');

            for (int i = 0; i < parts.Length; i++)
            {
                vers |= (ulong.Parse(parts[i]) << ((4 - (i + 1)) * 16));
            }

            return vers;
        }

        public static ReleaseChannel GetReleaseChannel()
        {
            ReleaseChannel result;

            if (Enum.TryParse<ReleaseChannel>(AppVM.GetSetting("app.releasechannel"), out result))
            {
                return result;
            }

            return ReleaseChannel.Official;
        }

        public static bool UpdateEnabled
        {
            get
            {
                bool result;

                if (Boolean.TryParse(AppVM.GetSetting("app.updateenabled"), out result))
                {
                    return result;
                }

                return true;
            }
        }

        public static bool CheckUpdateOnStart
        {
            get
            {
                bool result;

                if (Boolean.TryParse(AppVM.GetSetting("app.checkupdateonstart"), out result))
                {
                    return result;
                }

                return false;
            }
            set
            {
                AppVM.SetSetting("app.checkupdateonstart", value);
            }
        }

        public static bool IsConnectedToInternet
        {
            get
            {
                int Description;
                return InternetGetConnectedState(out Description, 0);
            }
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static DateTime LastUpdateCheck
        {
            get
            {
                DateTime result;

                if (DateTime.TryParse(AppVM.GetSetting("app.lastupdatecheck"), null, DateTimeStyles.AssumeUniversal, out result))
                {
                    return result;
                }

                return DateTime.MinValue;
            }
            set
            {
                AppVM.SetSetting("app.lastupdatecheck", value.ToUniversalTime().ToString("s"));
            }
        }

        private const string _updateUrl = "http://update.chordious.com";
        private const string _userAgent = "Mozilla/5.0";
    }

    public class UpdateException : Exception
    {
        public override string Message
        {
            get
            {
                return Strings.ChordiousUpdateExceptionMessage;
            }
        }

        public UpdateException(Exception innerException) : base("", innerException) { }
    }

    public class UpdateNoInternetException : Exception
    {
        public override string Message
        {
            get
            {
                return Strings.ChordiousUpdateNoInternetExceptionMessage;
            }
        }

        public UpdateNoInternetException() : base() { }
    }

    public class InstallerInfo
    {
        public string Version { get; private set; }

        public string Url { get; private set; }

        public ReleaseChannel ReleaseChannel { get; private set; }

        public InstallerInfo(string version, string url, ReleaseChannel releaseChannel)
        {
            if (String.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException("version");
            }

            if (String.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }

            Version = version.Trim();
            Url = url.Trim();
            ReleaseChannel = releaseChannel;
        }
    }

    public enum ReleaseChannel
    {
        Official,
        Preview
    }
}
