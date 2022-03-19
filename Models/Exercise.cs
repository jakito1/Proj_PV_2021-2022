using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string? ExerciseName { get; set; }
        public string? ExerciseDescription { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan? ExerciseDuration { get; set; }
        public int? ExerciseRepetitions { get; set; }
        public string? ExerciseURL { get; set; }
        public List<Picture>? Pictures { get; set; }
        public ExerciseType? ExerciseType { get; set; }
        public ExerciseMuscles? ExerciseMuscles { get; set; }
        public TrainingPlan? TrainingPlan { get; set; }

        [FromForm]
        [NotMapped]
        public IFormFileCollection? Files { get; set; }

    }

    public enum ExerciseType
    {
        CARDIO, STRENGHT
    }

    public enum ExerciseMuscles
    {
        LEG, ARM, BACK
    }
}
