using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    /// <summary>
    /// IPhotoManagement interface
    /// </summary>
    public interface IPhotoManagement
    {
        /// <summary>
        /// Method to upload a Profile photo.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Instance of a Photo class</returns>
        Photo UploadProfilePhoto(IFormFile? file);
        /// <summary>
        /// Loads the profile image.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>A string Task result</returns>
        Task<string> LoadProfileImage(string? userName);
        /// <summary>
        /// Gets the path for the photo.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>A string with the photo path</returns>
        string GetPhotoPath(Photo? photo);

    }
}
