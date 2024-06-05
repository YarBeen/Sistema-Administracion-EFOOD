using Azure.Storage.Blobs;

namespace SistemaEFood.Servicios
{
    public class StorageService : IStorageService
    {
        private readonly string _connectionString; 

        public StorageService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string containerName,
            string folderName, string fileName)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Create the container if it doesn't exist
                await blobContainerClient.CreateIfNotExistsAsync();

                // Combine folder name and file name using a delimiter (e.g., '/')
                var blobName = Path.Combine(folderName, fileName).Replace("\\", "/");

                // Get a reference to a blob
                var blobClient = blobContainerClient.GetBlobClient(blobName);

                // Upload the image
                //True es para que se sobreescriba
                
               await blobClient.UploadAsync(imageStream, true);
              




                // Get the full path of the uploaded file
                var blobUri = blobClient.Uri;
                Console.WriteLine("File uploaded " + blobUri);
                return blobUri.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

     
    }
}