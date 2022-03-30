using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class PhotoManagement : IPhotoManagement
    {
        private readonly ApplicationDbContext _context;

        public PhotoManagement(ApplicationDbContext context)
        {
            _context = context;

        }
        public Photo UploadProfilePhoto(IFormFile? file)
        {
            if (file is not null && string.Equals(Path.GetExtension(file.FileName), ".gif"))
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

        public async Task<string> LoadImage(int? clientId)
        {
            if (clientId is not null)
            {
                Photo photo = await _context.Client.Where(a => a.ClientId == clientId).Select(a => a.ClientProfilePhoto).FirstOrDefaultAsync();
                string imageBase64Data = Convert.ToBase64String(photo.PhotoData);
                return string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }
            return string.Empty;
        }
    }
}
