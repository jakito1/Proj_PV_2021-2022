using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class InteractNotification : IInteractNotification
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public InteractNotification(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
    ***REMOVED***

        public async Task Create(string? notificationMessage, UserAccountModel notificationReceiver)
        ***REMOVED***
            if (!string.IsNullOrEmpty(notificationMessage) && notificationReceiver is not null)
            ***REMOVED***
                await _context.Notifications.AddAsync(new Notification()
                ***REMOVED***
                    NotificationMessage = notificationMessage,
                    NotificationTime = DateTime.Now,
                    NotificationReceiver = notificationReceiver
            ***REMOVED***);
        ***REMOVED***
    ***REMOVED***
        public async Task<List<Notification>> GetLastThree(string? userName)
        ***REMOVED***
            UserAccountModel userAccount = await _userManager.FindByNameAsync(userName);
            List<Notification>? result = new();
            if (userAccount is not null)
            ***REMOVED***
                result = await _context.Notifications
                    .Where(a => a.NotificationReceiver == userAccount)
                    .OrderByDescending(a => a.NotificationTime)
                    .Take(3)
                    .ToListAsync();
        ***REMOVED***
            return result;
    ***REMOVED***

        public async Task<bool> NotificationsExist(string? userName)
        ***REMOVED***
            List<Notification>? notifications = await GetLastThree(userName);
            return notifications.Any();
    ***REMOVED***

***REMOVED***
***REMOVED***
