using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// Photo class
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// Gets and Sets the photo id.
        /// </summary>
        public int PhotoId { get; set; }
        /// <summary>
        /// Gets and Sets the photo title.
        /// </summary>
        public string? PhotoTitle { get; set; }
        /// <summary>
        /// Gets and Sets the byte code of the image.
        /// </summary>
        public byte[]? PhotoData { get; set; }
        /// <summary>
        /// Gets and Sets the photo url.
        /// </summary>
        [NotMapped]
        public string? PhotoUrl { get; set; }
    }
}
