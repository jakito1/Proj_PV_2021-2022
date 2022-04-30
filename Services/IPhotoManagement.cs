using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    /// <summary>
    /// IPhotoManagement interface
    /// </summary>
    public interface IPhotoManagement
    ***REMOVED***
        Photo? UploadProfilePhoto(IFormFile? file);

        Task<string> LoadProfileImage(string? userName);
        /// <summary>
        /// Gets the path for the photo.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>A string with the photo path</returns>
        string GetPhotoPath(Photo? photo);

***REMOVED***
***REMOVED***
