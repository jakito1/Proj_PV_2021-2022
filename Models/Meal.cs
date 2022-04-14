using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// Meal class, implements IValidatableObject
    /// </summary>
    public class Meal : IValidatableObject
    {
        /// <summary>
        /// Gets and Sets the Meal id.
        /// </summary>
        public int MealId { get; set; }
        /// <summary>
        /// Gets and Sets the Meal name.
        /// </summary>
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? MealName { get; set; }
        /// <summary>
        /// Gets and Sets the Meal description.
        /// </summary>
        public string? MealDescription { get; set; }
        /// <summary>
        /// Gets and Sets the Meal calories.
        /// </summary>
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre {1} e {2} calorias.")]
        public int? MealCalorie { get; set; }
        /// <summary>
        /// Gets and Sets the Meal protein value.
        /// </summary>
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre {1} e {2} gramas de proteína.")]
        public int? MealProtein { get; set; }
        /// <summary>
        /// Gets and Sets the Meal fat value.
        /// </summary>
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre {1} e {2} gramas de gordura.")]
        public int? MealFat { get; set; }
        /// <summary>
        /// Gets and Sets the Meal carbohydrate value.
        /// </summary>
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre {1} e {2} gramas de hidratos de carbono.")]
        public int? MealCarbohydrate { get; set; }
        /// <summary>
        /// Gets and Sets the Meal target date.
        /// </summary>

        [DataType(DataType.Date)]
        public DateTime? MealDate { get; set; }
        /// <summary>
        /// Gets the Sets the Meal target week day.
        /// </summary>

        [Column(TypeName = "nvarchar(24)")]
        public MealWeekDay? MealWeekDay { get; set; }
        /// <summary>
        /// Gets and Sets the Meal type.
        /// </summary>

        [Column(TypeName = "nvarchar(24)")]
        public MealType? MealType { get; set; }
        /// <summary>
        /// Gets and Sets the Meal url.
        /// </summary>

        [Url(ErrorMessage = "Este URL tem de estar no formato http, https, or ftp.")]
        public string? MealURL { get; set; }
        /// <summary>
        /// Gets and Sets the Meal associated nutrition plan.
        /// </summary>

        [JsonIgnore]
        public NutritionPlan? NutritionPlan { get; set; }
        /// <summary>
        /// Gets and Sets the Meal photo.
        /// </summary>
        public Photo? MealPhoto { get; set; }

        /// <summary>
        /// Validation function to validate the MealDate and MealWeekDay
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MealDate is null && MealWeekDay is null)
            {
                yield return new ValidationResult(
                    "Pelo menos uma data ou um dia de semana têm de ser fornecidos.",
                    new[] { nameof(MealDate), nameof(MealWeekDay) });
            }
            if (MealDate is not null && MealWeekDay is not null)
            {
                yield return new ValidationResult(
                    "Apenas pode fornecer uma data ou um dia de semana.",
                    new[] { nameof(MealDate), nameof(MealWeekDay) });
            }
            yield return ValidationResult.Success;
        }
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

    public enum MealWeekDay
    {
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
    }
}
