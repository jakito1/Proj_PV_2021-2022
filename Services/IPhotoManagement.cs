using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public interface IPhotoManagement
    ***REMOVED***
        Photo UploadProfilePhoto(IFormFile? file);

        Task<string> LoadProfileImage(string? userName);

        Task<string> LoadPhotoById(int? id);

***REMOVED***
***REMOVED***
