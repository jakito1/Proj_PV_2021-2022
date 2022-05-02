using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public class InteractNotification : IInteractNotification
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public InteractNotification(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<List<Notification>> GetLastThree(string? userName)
        {
            UserAccountModel userAccount = await _userManager.FindByNameAsync(userName);
            List<Notification>? result = new();
            if (userAccount is not null)
            {
                result = await _context.Notifications
                    .Where(a => a.NotificationReceiver == userAccount)
                    .OrderByDescending(a => a.NotificationTime)
                    .Take(3)
                    .ToListAsync();
            }
            return result;
        }

        public async Task<bool> NotificationsExist(string? userName)
        {
            List<Notification>? notifications = await GetLastThree(userName);
            return notifications.Any();
        }

    }
}
