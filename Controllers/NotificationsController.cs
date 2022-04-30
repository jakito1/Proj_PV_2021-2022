using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    [Authorize(Roles = "client, trainer, nutritionist, gym")]
    public class NotificationsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public NotificationsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
    ***REMOVED***

        public async Task<IActionResult> ShowNotifications(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***
            if (User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            if (searchString is not null)
            ***REMOVED***
                pageNumber = 1;
        ***REMOVED***
            else
            ***REMOVED***
                searchString = currentFilter;
        ***REMOVED***

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<Notification>? notifications = null;

            if (user is not null)
            ***REMOVED***
                notifications = _context.Notifications.Where(a => a.NotificationReceiver == user);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && user is not null)
            ***REMOVED***
                notifications = _context.Notifications.Where(a => a.NotificationReceiver == user)
                    .Where(a => a.NotificationMessage != null && a.NotificationMessage.Contains(searchString));
        ***REMOVED***

            if (notifications is not null)
            ***REMOVED***
                int pageSize = 5;
                return View(await PaginatedList<Notification>.CreateAsync(notifications.OrderByDescending(a => a.NotificationTime).AsNoTracking(), pageNumber ?? 1, pageSize));
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        public async Task<IActionResult> DeleteNotification(int? id)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Notification? notification = await _context.Notifications.Include(a => a.NotificationReceiver).FirstOrDefaultAsync(a => a.NotificationId == id);
            if (user is not null && notification is not null && notification.NotificationReceiver == user)
            ***REMOVED***
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
        ***REMOVED***
            return RedirectToAction("ShowNotifications");
    ***REMOVED***
        public async Task<IActionResult> RemoveAll()
        ***REMOVED***
            if (User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            IQueryable<Notification>? notifications = null;
            if (user is not null)
            ***REMOVED***
                notifications = _context.Notifications.Include(a => a.NotificationReceiver).Where(a => a.NotificationReceiver == user);
        ***REMOVED***
            if (user is not null && notifications is not null && notifications.Any())
            ***REMOVED***
                _context.Notifications.RemoveRange(notifications);
                await _context.SaveChangesAsync();
        ***REMOVED***

            return RedirectToAction("ShowNotifications");
    ***REMOVED***
***REMOVED***
***REMOVED***
