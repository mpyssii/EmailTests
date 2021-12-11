using Emails.Api.Consts;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Emails.Api
{
    public static class CleanupEmails
    {
        [FunctionName("cleanup-emails")]
        public static async Task Run([TimerTrigger("0 50 * * * *")] TimerInfo myTimer,
            [Blob(Constants.EmailContainerName, FileAccess.ReadWrite)] CloudBlobContainer myBlobContainer,
            ILogger log)
        {
            log.LogInformation($"{nameof(CleanupEmails)} Timer trigger function executed at: {DateTime.Now}");

            await myBlobContainer.CreateIfNotExistsAsync();

            BlobContinuationToken blobContinuationToken = null;
            var blobList = await myBlobContainer.ListBlobsSegmentedAsync(blobContinuationToken);
            var cloudBlobList = blobList.Results
                .Select(blb => blb as ICloudBlob)
                .Where(blb => blb.Properties.LastModified < DateTime.Now.AddHours(-1));

            foreach (var item in cloudBlobList)
            {
                await item.DeleteIfExistsAsync();
            }
        }
    }
}
