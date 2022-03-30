using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class PhotoManagement : IPhotoManagement
    ***REMOVED***
        private readonly ApplicationDbContext _context;

        public PhotoManagement(ApplicationDbContext context)
        ***REMOVED***
            _context = context;

    ***REMOVED***
        public Photo UploadProfilePhoto(IFormFile? file)
        ***REMOVED***
            if (file is not null && string.Equals(Path.GetExtension(file.FileName), ".gif"))
            ***REMOVED***
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
        ***REMOVED***
            return null;
    ***REMOVED***

        public async Task<string> LoadImage(int? clientId)
        ***REMOVED***
            if (clientId is not null)
            ***REMOVED***
                Photo photo = await _context.Client.Where(a => a.ClientId == clientId).Select(a => a.ClientProfilePhoto).FirstOrDefaultAsync();
                string imageBase64Data = Convert.ToBase64String(photo.PhotoData);
                return string.Format("data:image/jpg;base64,***REMOVED***0***REMOVED***", imageBase64Data);
        ***REMOVED***
            return string.Empty;
    ***REMOVED***
***REMOVED***
***REMOVED***
