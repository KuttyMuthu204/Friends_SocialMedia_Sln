using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Friends_Data.Helpers.Enums;
using Microsoft.AspNetCore.Http;

namespace Friends_Data.Services
{
    public class FilesService : IFilesService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public FilesService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadImageAsync(IFormFile file, ImageFileType fileType)
        {
            string containerPath = fileType switch
            {
                ImageFileType.PostImage => "posts",
                ImageFileType.StoryImage => "stories",
                ImageFileType.ProfilePicture => "profilepictures",
                ImageFileType.CoverImage => "covers",
                _ => throw new ArgumentException("Invalid file type")
            };

            if (file == null || file.Length == 0)
            {
                return "";
            }

            //ensure the container exists
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerPath);
            await containerClient.CreateIfNotExistsAsync();

            //generate a unique file name
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);

            //upload the file to the blob storage
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });
            }

            return blobClient.Uri.ToString();
        }
    }
}
