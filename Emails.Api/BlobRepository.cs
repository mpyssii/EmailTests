using Microsoft.Azure.WebJobs;
using System.IO;
using System.Threading.Tasks;
using Emails.Api.Consts;

namespace Emails.Api
{
    public class BlobRepository : IBlobRepository
    {
        public async Task<string> ReadEmail(IBinder binder, string blobName)
        {
            var attribute = new BlobAttribute(Constants.EmailContainerName + "/" + blobName);

            using var reader = await binder.BindAsync<TextReader>(attribute);

            return reader == null ? string.Empty : await reader.ReadToEndAsync();
        }
    }
}
