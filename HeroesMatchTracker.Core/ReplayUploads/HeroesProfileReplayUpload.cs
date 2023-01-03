using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Heroes.ReplayParser.DataParser;

namespace HeroesMatchTracker.Core.ReplayUploads
{
    public static class HeroesProfileReplayUpload
    {
        public static async Task<ReplayParseResult> UploadReplay(string filePath)
        {
            string response = string.Empty;

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://api.heroesprofile.com");

                    httpClient.DefaultRequestHeaders.UserAgent.Add(Uploader.UserAgentHeader);

                    MultipartFormDataContent dataContent = new MultipartFormDataContent();

                    using (var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath)))
                    {
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                        dataContent.Add(fileContent, "file", Path.GetFileName(filePath));

                        HttpResponseMessage responseMessage = await httpClient.PostAsync("api/upload/heroesprofile/heroesmatchtracker", dataContent);

                        response = await responseMessage.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception)
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
