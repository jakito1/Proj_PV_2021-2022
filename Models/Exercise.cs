using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório (máximo 20 caracteres)")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ExerciseName { get; set; }
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? ExerciseDescription { get; set; }

        [Range(1, 120, ErrorMessage = "Deve inserir entre {1} e {2} minutos.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Deve inserir um valor inteiro.")]
        public int? ExerciseDuration { get; set; }

        [Range(1, 999, ErrorMessage = "Deve inserir entre {1} e {2} repetições.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Deve inserir um valor inteiro.")]
        public int? ExerciseRepetitions { get; set; }

        [Url(ErrorMessage = "Este URL tem de estar no formato http, https, or ftp.")]
        public string? ExerciseURL { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public ExerciseType? ExerciseType { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public ExerciseMuscles? ExerciseMuscles { get; set; }

        [JsonIgnore]
        public TrainingPlan? TrainingPlan { get; set; }

        [JsonIgnore]
        public Machine? Machine { get; set; }

        public Photo? ExercisePhoto { get; set; }

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
