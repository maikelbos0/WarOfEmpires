using Azure.Storage.Blobs;
using System.IO;
using WarOfEmpires.Utilities.Configuration;

namespace WarOfEmpires.Utilities.Storage {
    public sealed class StorageClient : IStorageClient {
        private readonly AppSettings _appSettings;

        public StorageClient(AppSettings appSettings) {
            _appSettings = appSettings;
        }

        public void Delete(string fileName) {
            GetBlobContainerClient().DeleteBlobIfExists(fileName);
        }

        public void Store(string fileName, Stream content) {
            GetBlobContainerClient().UploadBlob(fileName, content);
        }

        private BlobContainerClient GetBlobContainerClient() {
            var serviceClient = new BlobServiceClient(_appSettings.BlobStorageConnectionString);
            
            return serviceClient.GetBlobContainerClient(_appSettings.BlobStorageContainer);
        }
    }
}
