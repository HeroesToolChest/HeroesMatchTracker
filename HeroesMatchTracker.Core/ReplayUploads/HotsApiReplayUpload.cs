using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Heroes.ReplayParser.DataParser;

namespace HeroesMatchTracker.Core.ReplayUploads
{
    public static class HotsApiReplayUpload
    {
        public static async Task<ReplayParseResult> UploadReplay(string filePath)
        {
            string response = string.Empty;

            try
            {
                using (var webClient = new WebClient())
                {
                    webClient.Headers.Add("user-agent", Uploader.AppUserAgent);
                    var bytes = await webClient.UploadFileTaskAsync($"http://hotsapi.net/api/v1/replays?uploadToHotslogs={true}", filePath);
                    response = Encoding.UTF8.GetString(bytes);
                }
            }
            catch (WebException)
            {
                return ReplayParseResult.Exception;
            }

            dynamic json = JObject.Parse(response);

            if ((bool)json.success)
            {
                if (Enum.TryParse((string)json.status, out HotsApiUploadStatus status))
                {
                    switch (status)
                    {
                        case HotsApiUploadStatus.Success:
                        case HotsApiUploadStatus.CustomGame:
                            return ReplayParseResult.Success;

                        case HotsApiUploadStatus.Duplicate:
                            return ReplayParseResult.Duplicate;

                        case HotsApiUploadStatus.AiDetected:
                            return ReplayParseResult.ComputerPlayerFound;

                        case HotsApiUploadStatus.PtrRegion:
                            return ReplayParseResult.PTRRegion;

                        case HotsApiUploadStatus.Incomplete:
                            return ReplayParseResult.Incomplete;

                        case HotsApiUploadStatus.None:
                        case HotsApiUploadStatus.InProgress:
                            return ReplayParseResult.UnexpectedResult;

                        case HotsApiUploadStatus.UploadError:
                            return ReplayParseResult.Exception;

                        case HotsApiUploadStatus.TooOld:
                            return ReplayParseResult.PreAlphaWipe;

                        default:
                            return ReplayParseResult.UnexpectedResult;
                    }
                }
                else
                {
                    return ReplayParseResult.Exception;
                }
            }
            else
            {
                return ReplayParseResult.Exception;
            }
        }
    }
}
