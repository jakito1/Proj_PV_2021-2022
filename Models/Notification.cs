namespace NutriFitWeb.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string? NotificationMessage { get; set; }
        public DateTime? NotificationTime { get; set; }
        public UserAccountModel? NotificationReceiver { get; set; }
    }
}
