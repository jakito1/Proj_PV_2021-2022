using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IInteractNotification
    {
        Task Create(string? notificationMessage, UserAccountModel notificationReceiver);
        Task<List<Notification>> GetLastFive(string? userName);
    }
}
