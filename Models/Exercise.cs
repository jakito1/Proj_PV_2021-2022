using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// Exercise class
    /// </summary>
    public class Exercise
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the exercise id.
        /// </summary>
        public int ExerciseId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the exercise name.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? ExerciseName ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the exercise description.
        /// </summary>
        public string? ExerciseDescription ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the exercise duration.
        /// </summary>
        [Range(1, 120, ErrorMessage = "Para um treino saudável, deve inserir entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** minutos.")]
        public int? ExerciseDuration ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the exercise repetitions.
        /// </summary>
        [Range(1, 999, ErrorMessage = "Para um treino saudável, deve inserir entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** repetições.")]
        public int? ExerciseRepetitions ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the exercise url.
        /// </summary>
        [Url(ErrorMessage = "Este URL tem de estar no formato http, https, or ftp.")]
        public string? ExerciseURL ***REMOVED*** get; set; ***REMOVED***
        /*public List<Picture>? Pictures ***REMOVED*** get; set; ***REMOVED****/

        /// <summary>
        /// Gets and Sets the exercise type.
        /// Types of: ExerciseType.CARDIO, ExerciseType.STRENGTH
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public ExerciseType? ExerciseType ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the exercise muscle group.
        /// Types of: ExerciseMuscles.LEGS,ExerciseMuscles.ARMS,ExerciseMuscles.BACK
        /// </summary>
        [Column(TypeName = "nvarchar(24)")]
        public ExerciseMuscles? ExerciseMuscles ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the current trining plan associated with the exercise.
        /// </summary>
        [JsonIgnore]
        public TrainingPlan? TrainingPlan ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the current machine associated with the exercise.
        /// </summary>
        [JsonIgnore]
        public Machine? Machine ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the exercise photo.
        /// </summary>
        public Photo? ExercisePhoto ***REMOVED*** get; set; ***REMOVED***

***REMOVED***

    /// <summary>
    /// Types of exercises, with 2 types.
    /// </summary>
    public enum ExerciseType
    ***REMOVED***
        [Display(Name = "Cardio")]
        CARDIO,
        [Display(Name = "Força")]
        STRENGHT
***REMOVED***

    /// <summary>
    /// Exercise muscles with 3 muscle groups.
    /// </summary>
    public enum ExerciseMuscles
    ***REMOVED***
        [Display(Name = "Pernas")]
        LEGS,
        [Display(Name = "Braços")]
        ARMS,
        [Display(Name = "Costas")]
        BACK
***REMOVED***
***REMOVED***
