using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    public class Exercise
    ***REMOVED***
        public int ExerciseId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? ExerciseName ***REMOVED*** get; set; ***REMOVED***
        public string? ExerciseDescription ***REMOVED*** get; set; ***REMOVED***

        [Range(1, 120, ErrorMessage = "Para um treino saudável, deve inserir entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** minutos.")]
        public int? ExerciseDuration ***REMOVED*** get; set; ***REMOVED***

        [Range(1, 999, ErrorMessage = "Para um treino saudável, deve inserir entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** repetições.")]
        public int? ExerciseRepetitions ***REMOVED*** get; set; ***REMOVED***
        public string? ExerciseURL ***REMOVED*** get; set; ***REMOVED***
        public List<Picture>? Pictures ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName = "nvarchar(24)")]
        public ExerciseType? ExerciseType ***REMOVED*** get; set; ***REMOVED***

        [Column(TypeName = "nvarchar(24)")]
        public ExerciseMuscles? ExerciseMuscles ***REMOVED*** get; set; ***REMOVED***

        [JsonIgnore]
        public TrainingPlan? TrainingPlan ***REMOVED*** get; set; ***REMOVED***

        [FromForm]
        [NotMapped]
        public IFormFileCollection? Files ***REMOVED*** get; set; ***REMOVED***

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
