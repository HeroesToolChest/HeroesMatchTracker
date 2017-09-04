using Amazon.S3;
using Amazon.S3.Model;
using Heroes.Helpers;
using System;
using System.Net;
using System.Threading.Tasks;
using static Heroes.ReplayParser.DataParser;

namespace HeroesMatchTracker.Core.ReplayUploads
{
    public static class HotsLogsReplayUpload
    {
        public static async Task<ReplayParseResult> UploadReplay(string filePath)
        {
            AmazonS3Client client = new AmazonS3Client("AKIAIESBHEUH4KAAG4UA", "LJUzeVlvw1WX1TmxDqSaIZ9ZU04WQGcshPQyp21x", Amazon.RegionEndpoint.USEast1);

            string fileGuidName = $"{Guid.NewGuid()}.StormReplay";

            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = "heroesreplays",
                Key = fileGuidName,
                FilePath = filePath,
            };

            PutObjectResponse response = await client.PutObjectAsync(request);

            if (response.HttpStatusCode != HttpStatusCode.OK)
                return ReplayParseResult.Exception;

            string result = string.Empty;

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", Uploader.AppUserAgent);
                result = await webClient.DownloadStringTaskAsync($"https://www.hotslogs.com/UploadFile?FileName={fileGuidName}&Source=HeroesMatchTracker");
            }

            if (result == "Maintenance")
                throw new MaintenanceException("Maintenance");

            return HeroesHelpers.EnumParser.ConvertReplayParseResultStringToEnum(result);
        }
    }
}
