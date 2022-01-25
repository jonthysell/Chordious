// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml;

using GalaSoft.MvvmLight.Messaging;

using Chordious.Core.ViewModel;

using Chordious.WPF.Resources;

namespace Chordious.WPF
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

        public static int TimeoutMS = 3000;

        public const int MaxTimeoutMS = 100000;

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

                List<UpdateInfo> updateInfos = GetLatestUpdateInfos();

                ReleaseChannel targetReleaseChannel = GetReleaseChannel();

                ulong maxVersion = LongVersion(AppVM.FullVersion);

                UpdateInfo latestVersion = null;

                bool updateAvailable = false;
                foreach (UpdateInfo updateInfo in updateInfos)
                {
                    if (updateInfo.ReleaseChannel <= targetReleaseChannel)
                    {
                        ulong installerVersion = LongVersion(updateInfo.Version);

                        if (installerVersion > maxVersion)
                        {
                            updateAvailable = true;
                            latestVersion = updateInfo;
                            maxVersion = installerVersion;
                        }
                    }
                }

                LastUpdateCheck = DateTime.Now;

                if (updateAvailable)
                {
                    if (confirmUpdate)
                    {
                        string message = string.Format(Strings.ChordiousUpdateAvailableUpdateNowMessageFormat, latestVersion.Version);
                        AppVM.AppView.DoOnUIThread(() =>
                        {
                            Messenger.Default.Send(new ConfirmationMessage(message, (confirmed) =>
                            {
                                try
                                {
                                    if (confirmed)
                                    {
                                        Messenger.Default.Send(new LaunchUrlMessage(latestVersion.Url));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ExceptionUtils.HandleException(new UpdateException(ex));
                                }
                            }));
                        });
                    }
                    else
                    {
                        AppVM.AppView.DoOnUIThread(() =>
                        {
                            Messenger.Default.Send(new LaunchUrlMessage(latestVersion.Url));
                        });
                    }
                }
                else
                {
                    if (showUpToDate)
                    {
                        AppVM.AppView.DoOnUIThread(() =>
                        {
                            Messenger.Default.Send(new ChordiousMessage(Strings.ChordiousUpdateNotAvailableMessage));
                        });
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.Timeout)
                {
                    TimeoutMS = (int)Math.Min(TimeoutMS * 1.5, MaxTimeoutMS);
                }

                if (showUpToDate)
                {
                    ExceptionUtils.HandleException(new UpdateException(ex));
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

        public static List<UpdateInfo> GetLatestUpdateInfos()
        {
            if (!IsConnectedToInternet)
            {
                throw new UpdateNoInternetException();
            }

            List<UpdateInfo> updateInfos = new List<UpdateInfo>();

            HttpWebRequest request = WebRequest.CreateHttp(_updateUrl);
            request.UserAgent = _userAgent;
            request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.Timeout = TimeoutMS;

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
                            updateInfos.Add(new UpdateInfo(version, url, releaseChannel));
                        }
                    }
                }
            }

            return updateInfos;
        }

        public static ulong LongVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException(nameof(version));
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
            try
            {
                return (ReleaseChannel)Enum.Parse(typeof(ReleaseChannel), AppVM.GetSetting("app.releasechannel"));
            }
            catch (Exception) { }

            return ReleaseChannel.Official;
        }

        public static bool UpdateEnabled
        {
            get
            {
#if UPDATES
                return true;
#else
                return false;
#endif
            }
        }

        public static bool CheckUpdateOnStart
        {
            get
            {
                try
                {
                    return bool.Parse(AppVM.GetSetting("app.checkupdateonstart"));
                }
                catch (Exception) { }

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
                return NativeMethods.InternetGetConnectedState(out _, 0);
            }
        }

        public static DateTime LastUpdateCheck
        {
            get
            {
                try
                {
                    return DateTime.Parse(AppVM.GetSetting("app.lastupdatecheck"), null, DateTimeStyles.AssumeUniversal);
                }
                catch (Exception) { }

                return DateTime.MinValue;
            }
            set
            {
                AppVM.SetSetting("app.lastupdatecheck", value.ToUniversalTime().ToString("s"));
            }
        }

        private const string _updateUrl = "https://gitcdn.link/cdn/jonthysell/Chordious/main/update.xml";
        private const string _userAgent = "Mozilla/5.0";
    }

    public class UpdateInfo
    {
        public string Version { get; private set; }

        public string Url { get; private set; }

        public ReleaseChannel ReleaseChannel { get; private set; }

        public UpdateInfo(string version, string url, ReleaseChannel releaseChannel)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
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

    [Serializable]
    public class UpdateException : Exception
    {
        public override string Message
        {
            get
            {
                string message = Strings.ChordiousUpdateExceptionMessage;
                if (InnerException is WebException wex)
                {
                    message = $"{message} ({wex.Status})";
                }
                return message;
            }
        }

        public UpdateException(Exception innerException) : base("", innerException) { }
    }

    [Serializable]
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

    internal static partial class NativeMethods
    {
        [DllImport("wininet.dll")]
        internal extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
    }
}
