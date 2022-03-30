using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    public class Meal : IValidatableObject
    ***REMOVED***
        public int MealId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? MealName ***REMOVED*** get; set; ***REMOVED***
        public string? MealDescription ***REMOVED*** get; set; ***REMOVED***
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** calorias.")]
        public int? MealCalorie ***REMOVED*** get; set; ***REMOVED***
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** gramas de proteína.")]
        public int? MealProtein ***REMOVED*** get; set; ***REMOVED***
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** gramas de gordura.")]
        public int? MealFat ***REMOVED*** get; set; ***REMOVED***
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** gramas de hidratos de carbono.")]
        public int? MealCarbohydrate ***REMOVED*** get; set; ***REMOVED***

        [DataType(DataType.Date)]
        public DateTime? MealDate ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName = "nvarchar(24)")]
        public MealWeekDay? MealWeekDay ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName = "nvarchar(24)")]
        public MealType? MealType ***REMOVED*** get; set; ***REMOVED***

        [Url(ErrorMessage = "Este URL tem de estar no formato http, https, or ftp.")]
        public string? MealURL ***REMOVED*** get; set; ***REMOVED***

        [JsonIgnore]
        public NutritionPlan? NutritionPlan ***REMOVED*** get; set; ***REMOVED***

        public Photo? MealProfilePhoto ***REMOVED*** get; set; ***REMOVED***


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        ***REMOVED***
            if (MealDate is null && MealWeekDay is null)
            ***REMOVED***
                yield return new ValidationResult(
                    "Pelo menos uma data ou um dia de semana têm de ser fornecidos.",
                    new[] ***REMOVED*** nameof(MealDate), nameof(MealWeekDay) ***REMOVED***);
        ***REMOVED***
            if (MealDate is not null && MealWeekDay is not null)
            ***REMOVED***
                yield return new ValidationResult(
                    "Apenas pode fornecer uma data ou um dia de semana.",
                    new[] ***REMOVED*** nameof(MealDate), nameof(MealWeekDay) ***REMOVED***);
        ***REMOVED***
            yield return ValidationResult.Success;
    ***REMOVED***
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

    public enum MealWeekDay
    ***REMOVED***
        [Display(Name = "Domingo")]
        SUNDAY,
        [Display(Name = "Segunda-Feira")]
        MONDAY,
        [Display(Name = "Terça-Feira")]
        TUESDAY,
        [Display(Name = "Quarta-Feira")]
        WEDNESDAY,
        [Display(Name = "Quinta-Feira")]
        THURSDAY,
        [Display(Name = "Sexta-Feira")]
        FRIDAY,
        [Display(Name = "Sábado")]
        SATURDAY
***REMOVED***
***REMOVED***
