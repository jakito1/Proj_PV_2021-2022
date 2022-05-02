namespace NutriFitWeb.Models
{
    /// <summary>
    /// ErrorViewModel class
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the RequestId.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets or sets the flag as false if the RequestId is null or empty.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    //this is a comment
}