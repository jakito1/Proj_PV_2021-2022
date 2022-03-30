using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class PhotoManagement : IPhotoManagement
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public PhotoManagement(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        public Photo UploadProfilePhoto(IFormFile? file)
        {
            if (file is not null && (string.Equals(Path.GetExtension(file.FileName), ".gif") || string.Equals(Path.GetExtension(file.FileName), ".jpg")))
            {
                Photo photo = new();
                photo.PhotoTitle = file.FileName;

                MemoryStream memoryStream = new();
                file.CopyTo(memoryStream);
                photo.PhotoData = memoryStream.ToArray();

                memoryStream.Close();
                memoryStream.Dispose();

                _context.Photos.Add(photo);
                _context.SaveChanges();
                return photo;
            }
            return null;
        }

        public async Task<string> LoadProfileImage(string? userName)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Trainer trainer = await _context.Trainer.Include(a => a.TrainerProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.Include(a => a.ClientProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.NutritionistProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            Photo? photo = null;

            if (client is not null && client.ClientProfilePhoto is not null && client.ClientProfilePhoto.PhotoData is not null)
            {
                photo = client.ClientProfilePhoto;
            }
            if (trainer is not null && trainer.TrainerProfilePhoto is not null && trainer.TrainerProfilePhoto.PhotoData is not null)
            {
                photo = trainer.TrainerProfilePhoto;
            }
            if (nutritionist is not null && nutritionist.NutritionistProfilePhoto is not null && nutritionist.NutritionistProfilePhoto.PhotoData is not null)
            {
                photo = nutritionist.NutritionistProfilePhoto;
            }
            if (photo is not null && photo.PhotoData is not null)
            {
                string imageBase64Data = Convert.ToBase64String(photo.PhotoData);
                return string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }
            return string.Empty;
        }

    }
}
