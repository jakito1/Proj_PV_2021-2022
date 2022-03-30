using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? ImageData { get; set; }
    }
}
