using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Este campo tem de ser maior que {1}.")]
        public int? ExerciseDuration { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "Este campo tem de ser maior que {1}.")]
        public int? ExerciseRepetitions { get; set; }
        public string? ExerciseURL { get; set; }
        public List<Picture>? Pictures { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public ExerciseType? ExerciseType { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public ExerciseMuscles? ExerciseMuscles { get; set; }

        [JsonIgnore]
        public TrainingPlan? TrainingPlan { get; set; }

        [FromForm]
        [NotMapped]
        public IFormFileCollection? Files { get; set; }

    }

    public enum ExerciseType
    {
        [Display(Name = "Cardio")]
        CARDIO,
        [Display(Name = "Força")]
        STRENGHT
    }

    public enum ExerciseMuscles
    {
        [Display(Name = "Pernas")]
        LEGS,
        [Display(Name = "Braços")]
        ARMS,
        [Display(Name = "Costas")]
        BACK
    }
}
