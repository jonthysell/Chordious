// Copyright (c) Jon Thysell <http://jonthysell.com>
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Messaging;

using Chordious.Core;
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
                return NetworkInterface.GetIsNetworkAvailable();
            }
        }

        public static bool IsCheckingforUpdate { get; private set; }

        public static ReleaseChannel ReleaseChannel
        {
            get
            {
                try
                {
                    return (ReleaseChannel)Enum.Parse(typeof(ReleaseChannel), AppVM.GetSetting("app.releasechannel"));
                }
                catch (Exception) { }

                return ReleaseChannel.Official;
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

        public const int MinTimeoutMS = 3000;

        public const int MaxTimeoutMS = 100000;

        public static async Task UpdateCheckAsync(bool confirmUpdate, bool showUpToDate)
        {
            try
            {
                IsCheckingforUpdate = true;

                var latestRelease = await GetLatestGitHubReleaseInfoAsync("jonthysell", AppInfo.Product, showUpToDate ? MaxTimeoutMS : MinTimeoutMS);

                if (latestRelease is null)
                {
                    if (showUpToDate)
                    {
                        Messenger.Default.Send(new ChordiousMessage(Strings.ChordiousUpdateExceptionMessage));
                    }
                }
                else if (latestRelease.LongVersion <= AppInfo.LongVersion)
                {
                    if (showUpToDate)
                    {
                        Messenger.Default.Send(new ChordiousMessage(Strings.ChordiousUpdateNotAvailableMessage));
                    }
                }
                else
                {
                    // Update available
                    if (confirmUpdate)
                    {
                        Messenger.Default.Send(new ConfirmationMessage(string.Format(Strings.ChordiousUpdateAvailableUpdateNowMessageFormat, latestRelease.TagName), (result) =>
                        {
                            try
                            {
                                if (result)
                                {
                                    Messenger.Default.Send(new LaunchUrlMessage(latestRelease.HtmlUrl.AbsoluteUri));
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionUtils.HandleException(ex);
                            }

                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionUtils.HandleException(new UpdateException(ex));
            }
            finally
            {
                LastUpdateCheck = DateTime.Now;
                IsCheckingforUpdate = false;
            }
        }

        private static async Task<GitHubReleaseInfo> GetLatestGitHubReleaseInfoAsync(string owner, string repo, int timeoutMS = MinTimeoutMS)
        {
            var releaseInfos = await GetGitHubReleaseInfosAsync(owner, repo, timeoutMS);

            var includePrereleases = ReleaseChannel == ReleaseChannel.Preview;

            return releaseInfos
                .Where(info => info.Prerelease == false || includePrereleases)
                .OrderByDescending(info => info.LongVersion)
                .ThenBy(info => info.Name)
                .FirstOrDefault();
        }

        private static async Task<IList<GitHubReleaseInfo>> GetGitHubReleaseInfosAsync(string owner, string repo, int timeoutMS = MinTimeoutMS)
        {
            if (!IsConnectedToInternet)
            {
                throw new UpdateNoInternetException();
            }

            var releaseInfos = new List<GitHubReleaseInfo>();

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github.v3+json");
                client.DefaultRequestHeaders.UserAgent.ParseAdd(_userAgent);
                client.Timeout = TimeSpan.FromMilliseconds(timeoutMS);

                using var responseStream = await client.GetStreamAsync($"https://api.github.com/repos/{owner}/{repo}/releases");
                var jsonDocument = await JsonDocument.ParseAsync(responseStream);

                foreach (var releaseObject in jsonDocument.RootElement.EnumerateArray())
                {
                    string name = releaseObject.GetProperty("name").GetString();
                    string tagName = releaseObject.GetProperty("tag_name").GetString();
                    string htmlUrl = releaseObject.GetProperty("html_url").GetString();
                    bool draft = releaseObject.GetProperty("draft").GetBoolean();
                    bool prerelease = releaseObject.GetProperty("prerelease").GetBoolean();

                    releaseInfos.Add(new GitHubReleaseInfo(name, tagName, htmlUrl, draft, prerelease));
                }
            }
            catch (Exception) { }

            return releaseInfos;
        }

        private const string _userAgent = "Mozilla/5.0";
    }

    public class GitHubReleaseInfo
    {
        public readonly string Name;
        public readonly string TagName;
        public readonly Uri HtmlUrl;
        public readonly bool Draft;
        public readonly bool Prerelease;

        public ulong LongVersion
        {
            get
            {
                if (!_longVersion.HasValue && VersionUtils.TryParseLongVersion(TagName, out ulong result))
                {
                    _longVersion = result;
                }
                return _longVersion.Value;
            }
        }
        private ulong? _longVersion;

        public GitHubReleaseInfo(string name, string tagName, string htmlUrl, bool draft, bool prerelease)
        {
            Name = name?.Trim() ?? throw new ArgumentNullException(nameof(name));
            TagName = tagName?.Trim() ?? throw new ArgumentNullException(nameof(tagName));
            HtmlUrl = new Uri(htmlUrl);
            Draft = draft;
            Prerelease = prerelease;
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
                if (InnerException is HttpRequestException hre)
                {
                    message = $"{message} ({hre.StatusCode})";
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
}