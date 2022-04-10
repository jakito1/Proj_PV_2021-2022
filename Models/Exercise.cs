using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    public class Exercise
    ***REMOVED***
        public int ExerciseId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório (máximo 20 caracteres)")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ExerciseName ***REMOVED*** get; set; ***REMOVED***
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? ExerciseDescription ***REMOVED*** get; set; ***REMOVED***

        [Range(1, 120, ErrorMessage = "Para um treino saudável, deve inserir entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** minutos.")]
        public int? ExerciseDuration ***REMOVED*** get; set; ***REMOVED***

        [Range(1, 999, ErrorMessage = "Para um treino saudável, deve inserir entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** repetições.")]
        public int? ExerciseRepetitions ***REMOVED*** get; set; ***REMOVED***

        [Url(ErrorMessage = "Este URL tem de estar no formato http, https, or ftp.")]
        public string? ExerciseURL ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName = "nvarchar(24)")]
        public ExerciseType? ExerciseType ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName = "nvarchar(24)")]
        public ExerciseMuscles? ExerciseMuscles ***REMOVED*** get; set; ***REMOVED***

        [JsonIgnore]
        public TrainingPlan? TrainingPlan ***REMOVED*** get; set; ***REMOVED***

        [JsonIgnore]
        public Machine? Machine ***REMOVED*** get; set; ***REMOVED***

        public Photo? ExercisePhoto ***REMOVED*** get; set; ***REMOVED***

***REMOVED***

    public enum ExerciseType
    ***REMOVED***
        [Display(Name = "Cardio")]
        CARDIO,
        [Display(Name = "Força")]
        STRENGHT
***REMOVED***

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
