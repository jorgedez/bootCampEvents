using Microsoft.WindowsAzure.Storage;
using System.Configuration;
using System.Threading.Tasks;

namespace bootCamp.Shared.Helpers
{
    public static class BlobHelper
    {
        public static async Task<string> GetAsText(string fileName, string container) 
        {
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureWebJobsStorage"]);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var privateContainer = blobClient.GetContainerReference(container);
                var jsonBlob = privateContainer.GetBlockBlobReference(fileName);

                return await jsonBlob.DownloadTextAsync();
        }
    }
}
