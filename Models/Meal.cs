﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    public class Meal : IValidatableObject
    {
        public int MealId { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? MealName { get; set; }
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? MealDescription { get; set; }
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre {1} e {2} calorias.")]
        public int? MealCalorie { get; set; }
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre {1} e {2} gramas de proteína.")]
        public int? MealProtein { get; set; }
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre {1} e {2} gramas de gordura.")]
        public int? MealFat { get; set; }
        [Range(1, 99999, ErrorMessage = "Uma refeição deve conter entre {1} e {2} gramas de hidratos de carbono.")]
        public int? MealCarbohydrate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MealDate { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public MealWeekDay? MealWeekDay { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public MealType? MealType { get; set; }

        [Url(ErrorMessage = "Este URL tem de estar no formato http, https, or ftp.")]
        public string? MealURL { get; set; }

        [JsonIgnore]
        public NutritionPlan? NutritionPlan { get; set; }

        public Photo? MealPhoto { get; set; }


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