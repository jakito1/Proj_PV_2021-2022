using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class Meal
    ***REMOVED***
        public int MealId ***REMOVED*** get; set; ***REMOVED***
        [Required]
        public string? MealName ***REMOVED*** get; set; ***REMOVED***
        public string? MealDescription ***REMOVED*** get; set; ***REMOVED***
        public int? MealCalorie ***REMOVED*** get; set; ***REMOVED***
        public int? MealProtein ***REMOVED*** get; set; ***REMOVED***
        public int? MealFat ***REMOVED*** get; set; ***REMOVED***
        public int? MealCarbohydrate ***REMOVED*** get; set; ***REMOVED***

        [DataType(DataType.Date)]
        public DateTime? MealDate ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName = "nvarchar(24)")]
        public MealType? MealType ***REMOVED*** get; set; ***REMOVED***
        public List<Picture>? Pictures ***REMOVED*** get; set; ***REMOVED***

        [FromForm]
        [NotMapped]
        public IFormFileCollection? Files ***REMOVED*** get; set; ***REMOVED***

***REMOVED***

    public enum MealType
    ***REMOVED***
        [Display(Name = "Pequeno Almoço")]
        BREAKFAST,
        [Display(Name = "Almoço")]
        LUNCH,
        [Display(Name = "Jantar")]
        DINER
***REMOVED***
***REMOVED***
