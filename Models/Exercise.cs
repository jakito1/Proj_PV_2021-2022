using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// Exercise class
    /// </summary>
    public class Exercise
    {
        /// <summary>
        /// Gets and Sets the exercise id.
        /// </summary>
        public int ExerciseId { get; set; }
        /// <summary>
        /// Gets and Sets the exercise name.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Required(ErrorMessage = "Campo Obrigatório (máximo 20 caracteres)")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ExerciseName { get; set; }
        /// <summary>
        /// Gets and Sets the exercise description.
        /// </summary>
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? ExerciseDescription { get; set; }

        /// <summary>
        /// Gets and Sets the exercise duration.
        /// </summary>
        [Range(1, 120, ErrorMessage = "Para um treino saudável, deve inserir entre {1} e {2} minutos.")]
        public int? ExerciseDuration { get; set; }

        /// <summary>
        /// Gets and Sets the exercise repetitions.
        /// </summary>
        [Range(1, 999, ErrorMessage = "Para um treino saudável, deve inserir entre {1} e {2} repetições.")]
        public int? ExerciseRepetitions { get; set; }

        /// <summary>
        /// Gets and Sets the exercise url.
        /// </summary>
        [Url(ErrorMessage = "Este URL tem de estar no formato http, https, or ftp.")]
        public string? ExerciseURL { get; set; }

        /// <summary>
        /// Gets and Sets the exercise type.
        /// Types of: ExerciseType.CARDIO, ExerciseType.STRENGTH
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public ExerciseType? ExerciseType { get; set; }

        /// <summary>
        /// Gets and Sets the exercise muscle group.
        /// Types of: ExerciseMuscles.LEGS,ExerciseMuscles.ARMS,ExerciseMuscles.BACK
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public ExerciseMuscles? ExerciseMuscles { get; set; }

        /// <summary>
        /// Gets and Sets the current trining plan associated with the exercise.
        /// </summary>
        [JsonIgnore]
        public TrainingPlan? TrainingPlan { get; set; }

        /// <summary>
        /// Gets and Sets the current machine associated with the exercise.
        /// </summary>
        [JsonIgnore]
        public Machine? Machine { get; set; }

        /// <summary>
        /// Gets and Sets the exercise photo.
        /// </summary>
        public Photo? ExercisePhoto { get; set; }

    }

    /// <summary>
    /// Types of exercises, with 2 types.
    /// </summary>
    public enum ExerciseType
    {
        [Display(Name = "Cardio")]
        CARDIO,
        [Display(Name = "Força")]
        STRENGHT
    }

    /// <summary>
    /// Exercise muscles with 3 muscle groups.
    /// </summary>
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
