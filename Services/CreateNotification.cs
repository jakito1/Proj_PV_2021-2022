using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class CreateNotification : ICreateNotification
    {
        private readonly ApplicationDbContext _context;

        public CreateNotification(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Create(string? notificationMessage, UserAccountModel notificationReceiver)
        {
            if (!string.IsNullOrEmpty(notificationMessage) && notificationReceiver is not null)
            {
                await _context.Notifications.AddAsync(new Notification() 
                { 
                    NotificationMessage = notificationMessage,
                    NotificationTime = DateTime.Now,
                    NotificationReceiver = notificationReceiver 
                });
            }
        }

    }
}
