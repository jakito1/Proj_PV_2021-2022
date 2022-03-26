using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        [Required]
        public string? MealName { get; set; }
        public string? MealDescription { get; set; }
        public int? MealCalorie { get; set; }
        public int? MealProtein { get; set; }
        public int? MealFat { get; set; }
        public int? MealCarbohydrate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MealDate { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public MealType? MealType { get; set; }
        public List<Picture>? Pictures { get; set; }

        [FromForm]
        [NotMapped]
        public IFormFileCollection? Files { get; set; }

    }

    public enum MealType
    {
        [Display(Name = "Pequeno Almoço")]
        BREAKFAST,
        [Display(Name = "Almoço")]
        LUNCH,
        [Display(Name = "Jantar")]
        DINER
    }
}
