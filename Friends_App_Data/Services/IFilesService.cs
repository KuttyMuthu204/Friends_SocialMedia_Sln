using Friends_App_Data.Helpers.Enums;
using Microsoft.AspNetCore.Http;

namespace Friends_App_Data.Services
{
    public interface IFilesService
    {
        Task<string> UploadImageAsync(IFormFile file, ImageFileType fileType);
    }
}
