using Amazon.S3;
using Amazon.S3.Model;
using Heroes.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static Heroes.ReplayParser.DataParser;

namespace HeroesStatTracker.Core.HotsLogs
{
    public static class HotsLogsUploader
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
                throw new Exception("Failed to upload");

            using (HttpClient httpClient = new HttpClient())
            {
                string result = await httpClient.GetStringAsync($"https://www.hotslogs.com/UploadFile?FileName={fileGuidName}");

                if (result == "Maintenance")
                    throw new MaintenanceException("Maintenance");

                return HeroesHelpers.EnumParser.ConvertReplayParseResultStringToEnum(result);
            }
        }
    }
}
