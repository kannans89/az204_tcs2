using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SeondFunctionApp
{
    public class BlobTriggerFunc
    {
        private readonly ILogger<BlobTriggerFunc> _logger;

        public BlobTriggerFunc(ILogger<BlobTriggerFunc> logger)
        {
            _logger = logger;
        }

        [Function(nameof(BlobTriggerFunc))]
        public async Task Run([BlobTrigger("mydocuments/{name}", Connection = "myConnectionString")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {content}");
        }
    }
}
