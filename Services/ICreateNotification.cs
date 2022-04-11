using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface ICreateNotification
    {
        Task Create(string? notificationMessage, UserAccountModel notificationReceiver);
    }
}
