using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
***REMOVED***
    public class Meal
    ***REMOVED***
        public int MealId ***REMOVED*** get; set; ***REMOVED***
        [Required]
        public string? MealName ***REMOVED*** get; set; ***REMOVED***
        public string? MealDescription ***REMOVED*** get; set; ***REMOVED***
        public int? MealCalories ***REMOVED*** get; set; ***REMOVED***

        [DataType(DataType.Time)]
        public TimeSpan? MealTime ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
