namespace Cosiness.Services.Storage
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;

    using System.IO;
    using System.Threading.Tasks;

    public class BlobStorageService : IFileStorage
    {
        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=chestorage;AccountKey=LONQdkj/LDSeC6AXSyoFvloOnBCpc6URtU37GA+Prv7xnfKIcF1FuyinfMR1lV42jTDzsmuE4sg5At02GO55wQ==;EndpointSuffix=core.windows.net";
        private readonly string _containerName = "cosiness";
        private readonly BlobContainerClient _container;

        public BlobStorageService()
        {
            _container = new BlobContainerClient(_connectionString, _containerName);
        }

        public async Task<string> UploadAsync(string FileName, Stream content)
        {
            var blob = _container.GetBlobClient(FileName);

            await blob.UploadAsync(content);

            return blob.Uri.AbsoluteUri;
        }

        public async Task DeleteAsync(string FileName)
        {
            var blob = _container.GetBlobClient(FileName);

            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
    }
}