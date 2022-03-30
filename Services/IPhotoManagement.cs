using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IPhotoManagement
    {
        Photo UploadProfilePhoto(IFormFile? file);

        Task<string> LoadProfileImage(string? userName);

    }
}
