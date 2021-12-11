using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace Emails.Api
{
    public interface IBlobRepository
    {
        Task<string> ReadEmail(IBinder binder, string blobName);
    }
}
