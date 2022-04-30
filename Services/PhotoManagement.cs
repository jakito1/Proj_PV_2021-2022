using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    /// <summary>
    /// PhotoManagement class, implements IPhotoManagement
    /// </summary>
    public class PhotoManagement : IPhotoManagement
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public PhotoManagement(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;

    ***REMOVED***
        public Photo? UploadProfilePhoto(IFormFile? file)
        ***REMOVED***
            if (file is not null && (string.Equals(Path.GetExtension(file.FileName), ".gif")
                                        || string.Equals(Path.GetExtension(file.FileName), ".png")
                                        || string.Equals(Path.GetExtension(file.FileName), ".PNG")
                                        || string.Equals(Path.GetExtension(file.FileName), ".jpg")
                                        || string.Equals(Path.GetExtension(file.FileName), ".jpeg"))
               )
            ***REMOVED***
                Photo photo = new();
                photo.PhotoTitle = file.FileName;

                MemoryStream memoryStream = new();
                file.CopyTo(memoryStream);
                photo.PhotoData = memoryStream.ToArray();

                memoryStream.Close();
                memoryStream.Dispose();

                return photo;
        ***REMOVED***
            return null;
    ***REMOVED***

        public async Task<string> LoadProfileImage(string? userName)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Client? client = await _context.Client.Include(a => a.ClientProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Trainer? trainer = await _context.Trainer.Include(a => a.TrainerProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.NutritionistProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Gym? gym = await _context.Gym.Include(a => a.GymProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            Photo? photo = null;

            if (client is not null && client.ClientProfilePhoto is not null && client.ClientProfilePhoto.PhotoData is not null)
            ***REMOVED***
                photo = client.ClientProfilePhoto;
        ***REMOVED***
            if (trainer is not null && trainer.TrainerProfilePhoto is not null && trainer.TrainerProfilePhoto.PhotoData is not null)
            ***REMOVED***
                photo = trainer.TrainerProfilePhoto;
        ***REMOVED***
            if (nutritionist is not null && nutritionist.NutritionistProfilePhoto is not null && nutritionist.NutritionistProfilePhoto.PhotoData is not null)
            ***REMOVED***
                photo = nutritionist.NutritionistProfilePhoto;
        ***REMOVED***

            if (gym is not null && gym.GymProfilePhoto is not null && gym.GymProfilePhoto.PhotoData is not null)
            ***REMOVED***
                photo = gym.GymProfilePhoto;
        ***REMOVED***

            if (photo is not null && photo.PhotoData is not null)
            ***REMOVED***
                string imageBase64Data = Convert.ToBase64String(photo.PhotoData);
                return string.Format("data:image/jpg;base64,***REMOVED***0***REMOVED***", imageBase64Data);
        ***REMOVED***
            return string.Empty;
    ***REMOVED***

        public string GetPhotoPath(Photo? photo)
        ***REMOVED***
            if (photo is not null && photo.PhotoData is not null)
            ***REMOVED***
                string imageBase64Data = Convert.ToBase64String(photo.PhotoData);
                return string.Format("data:image/jpg;base64,***REMOVED***0***REMOVED***", imageBase64Data);
        ***REMOVED***
            return string.Empty;
    ***REMOVED***
***REMOVED***
***REMOVED***
