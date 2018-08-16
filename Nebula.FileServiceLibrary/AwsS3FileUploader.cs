using System;
using System.IO;
using System.Net.Http.Headers;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Nebula.CoreLibrary;
using Nebula.CoreLibrary.Shared;

namespace Nebula.FileServiceLibrary
{
    public class AwsS3FileUploader : IFileUploader
    {
        public string UploadFile(IFormFile formFile)
        {
            string bucketName = ApplicationSettings.TryGetValueFromAppSettings("awsBucketName");
            string endpointString = ApplicationSettings.TryGetValueFromAppSettings("awsEndpoint");
            if (formFile.Length <= 0) return string.Empty;
            var filename = ContentDispositionHeaderValue
                .Parse(formFile.ContentDisposition)
                .FileName
                .TrimStart().ToString();
            filename = formFile.FileName;
            UploadToS3(formFile.OpenReadStream(), bucketName, filename);
            string endpoint = $"{endpointString}/" + bucketName + "/" + filename;
            return endpoint;

        }

        private void UploadToS3(Stream stream, string bucketName, string filename)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(
                ApplicationSettings.TryGetValueFromAppSettings("accessKey"),
                ApplicationSettings.TryGetValueFromAppSettings("secretKey"));

            RegionEndpoint endPoint = (RegionEndpoint)ApplicationSettings.AppSettings["regionEndpoint"];
            AmazonS3Client client = new AmazonS3Client(awsCredentials, endPoint ?? RegionEndpoint.EUCentral1);
            TransferUtility fileTransferUtility = new TransferUtility(client);
            fileTransferUtility.Upload(stream, bucketName, filename);
        }

        private void UploadToS3(string filePath, string bucketName)
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(
                ApplicationSettings.TryGetValueFromAppSettings("accessKey"),
                ApplicationSettings.TryGetValueFromAppSettings("secretKey"));
            AmazonS3Client client = new AmazonS3Client(awsCredentials, RegionEndpoint.EUCentral1);
            TransferUtility fileTransferUtility = new TransferUtility(client);
            filePath = "/UploadedFiles/" + filePath;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                fileTransferUtility.Upload(stream, bucketName, filePath);
            }
        }
    }
}
