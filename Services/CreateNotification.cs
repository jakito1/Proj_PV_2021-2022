using Microsoft.AspNetCore.Identity;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class CreateNotification : ICreateNotification
    ***REMOVED***
        private readonly ApplicationDbContext _context;

        public CreateNotification(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
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

***REMOVED***
***REMOVED***
