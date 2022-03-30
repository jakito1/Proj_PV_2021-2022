using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string? PhotoTitle { get; set; }
        public byte[]? PhotoData { get; set; }

        [NotMapped]
        public string? PhotoUrl { get; set; }
    }
}
