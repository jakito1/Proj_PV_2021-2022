using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public NotificationsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ShowNotifications(string? searchString, string? currentFilter, int? pageNumber)
        {

            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<Notification>? notifications = null;

            if (user is not null)
            {
                notifications = _context.Notifications.Where(a => a.NotificationReceiver == user);
            }

            if (!string.IsNullOrEmpty(searchString) && user is not null)
            {
                notifications = _context.Notifications.Where(a => a.NotificationReceiver == user)
                    .Where(a => a.NotificationMessage.Contains(searchString));
            }

            int pageSize = 5;
            return View(await PaginatedList<Notification>.CreateAsync(notifications.OrderByDescending(a => a.NotificationTime).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> DeleteNotification(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Notification? notification = await _context.Notifications.Include(a => a.NotificationReceiver).FirstOrDefaultAsync(a => a.NotificationId == id);
            if (user is not null && notification is not null && notification.NotificationReceiver == user)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowNotifications");
        }
        public async Task<IActionResult> RemoveAll()
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            IQueryable<Notification>? notifications = null;
            if (user is not null)
            {
                notifications = _context.Notifications.Include(a => a.NotificationReceiver).Where(a => a.NotificationReceiver == user);
            }            
            if (user is not null && notifications is not null && notifications.Any())
            {
                _context.Notifications.RemoveRange(notifications);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ShowNotifications");
        }

    }
}
