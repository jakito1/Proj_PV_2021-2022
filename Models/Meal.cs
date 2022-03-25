using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class Meal
    {
        public int MealId { get; set; }
        [Required]
        public string? MealName { get; set; }
        public string? MealDescription { get; set; }
        public int? MealCalories { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? MealTime { get; set; }
    }
}
