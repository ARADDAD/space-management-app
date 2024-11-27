
using Azure;
using Azure.Storage.Blobs;
using BlobDataApi.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BlobDataApi.Services
{
    public class BlobStorageService
    {
        private readonly BlobStorageConfig _config;

        public BlobStorageService(IOptions<BlobStorageConfig> config)
        {
            _config = config.Value;
        }

        public async Task<T> GetBlobDataAsync<T>()
        {
            try
            {
                // Construct the full URL with SAS Token
                string fullBlobUrl = $"{_config.BlobUrl}{_config.SasToken}";

                // Create the BlobClient
                var blobClient = new BlobClient(new Uri(fullBlobUrl));

                // Check if the blob exists
                if (!await blobClient.ExistsAsync())
                {
                    throw new FileNotFoundException("Blob not found.");
                }

                // Download the blob
                var response = await blobClient.DownloadAsync();
                using (var reader = new System.IO.StreamReader(response.Value.Content))
                {
                    string json = await reader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new InvalidOperationException($"File error: {ex.Message}", ex);
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == "AuthenticationFailed")
            {
                throw new UnauthorizedAccessException("Invalid SAS token or insufficient permissions.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error: {ex.Message}", ex);
            }
        }

    }

}
