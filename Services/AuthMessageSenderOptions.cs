namespace NutriFitWeb.Services
{
    /// <summary>
    /// AuthMessageSenderOptions class, responsible for managing the SendGrid key
    /// </summary>
    public class AuthMessageSenderOptions
    {
        /// <summary>
        /// Gets or sets the private SendGridKey
        /// </summary>
        public string? SendGridKey { get; set; }
    }
}
