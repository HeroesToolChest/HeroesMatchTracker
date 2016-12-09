using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Net;
using System.Threading.Tasks;
using static Heroes.ReplayParser.DataParser;

namespace HeroesParserData.HotsLogs
{
    public class HotsLogsUploader
    {
        private AmazonS3Client AmazonS3Client = new AmazonS3Client("AKIAIESBHEUH4KAAG4UA", "LJUzeVlvw1WX1TmxDqSaIZ9ZU04WQGcshPQyp21x", Amazon.RegionEndpoint.USEast1);

        public async Task<ReplayParseResult> UploadReplay(string filePath)
        {
            string fileGuidName = $"{Guid.NewGuid()}.StormReplay";

            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = "heroesreplays",
                Key = fileGuidName,
                FilePath = filePath
            };

            PutObjectResponse response = await AmazonS3Client.PutObjectAsync(request);

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new Exception("Failed to upload");

            using (WebClient webClient = new WebClient())
            {
                string result = webClient.DownloadString("https://www.hotslogs.com/UploadFile?FileName=" + fileGuidName);

                if (result == "Maintenance")
                    throw new MaintenanceException("Maintenance");

                return Utilities.GetReplayParseResultFromString(result);
            }
        }
    }

    [Serializable]
    public class MaintenanceException : Exception
    {
        public MaintenanceException(string message, Exception ex)
            :base(message, ex)
        {

        }

        public MaintenanceException(string message)
            : base(message)
        {

        }
    }

}
