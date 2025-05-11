using Friends_App_Data.Helpers.Enums;
using Microsoft.AspNetCore.Http;

namespace Friends_App_Data.Services
{
    public class FilesService : IFilesService
    {
        public async Task<string> UploadImageAsync(IFormFile file, ImageFileType fileType)
        {
            string filePathUpload = fileType switch
            {
                ImageFileType.PostImage => "images/posts",
                ImageFileType.StoryImage => "images/stories",
                ImageFileType.ProfilePicture => "images/profilePictures",
                ImageFileType.CoverImage => "images/covers",
                _ => throw new ArgumentException("Invalid file type")
            };

            if (file != null && file.Length > 0)
            {
                string rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                if (file.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, filePathUpload);
                    Directory.CreateDirectory(rootFolderPathImages);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    //Set the URL to the newPost object
                    return $"/{filePathUpload}/{fileName}";
                }
            }

            return "";
        }
    }
}
