using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;
using Emails.Api.Consts;

namespace Emails.Api
{
    public class GetEmail
    {
        private readonly IBlobRepository _blobRepository;

        public GetEmail(IBlobRepository blobRepository)
        {
            _blobRepository = blobRepository;
        }

        [FunctionName("get-email")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Blob(Constants.EmailContainerName, FileAccess.Read)] CloudBlobContainer myBlobContainer,
            ILogger log,
            IBinder binder)
        {
            log.LogInformation("C# HTTP trigger GetEmail function starts processing a request.");

            await myBlobContainer.CreateIfNotExistsAsync();

            string orderId = req.Query["orderid"];

            if (string.IsNullOrEmpty(orderId))
                return new BadRequestObjectResult("Please pass an orderId on the query string");

            var result = await _blobRepository.ReadEmail(binder, orderId);

            return new OkObjectResult(result);
        }
    }
}
