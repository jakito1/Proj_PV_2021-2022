﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    public class Exercise
    ***REMOVED***
        public int ExerciseId ***REMOVED*** get; set; ***REMOVED***
        public string? ExerciseName ***REMOVED*** get; set; ***REMOVED***
        public string? ExerciseDescription ***REMOVED*** get; set; ***REMOVED***
        [DataType(DataType.Time)]
        public TimeSpan? ExerciseDuration ***REMOVED*** get; set; ***REMOVED***
        public int? ExerciseRepetitions ***REMOVED*** get; set; ***REMOVED***
        public string? ExerciseURL ***REMOVED*** get; set; ***REMOVED***
        public List<Picture>? Pictures ***REMOVED*** get; set; ***REMOVED***
        public ExerciseType? ExerciseType ***REMOVED*** get; set; ***REMOVED***
        public ExerciseMuscles? ExerciseMuscles ***REMOVED*** get; set; ***REMOVED***

        [JsonIgnore]
        public TrainingPlan? TrainingPlan ***REMOVED*** get; set; ***REMOVED***

        [FromForm]
        [NotMapped]
        public IFormFileCollection? Files ***REMOVED*** get; set; ***REMOVED***

***REMOVED***

    public enum ExerciseType
    ***REMOVED***
        CARDIO, STRENGHT
***REMOVED***

    public enum ExerciseMuscles
    ***REMOVED***
        LEG, ARM, BACK
***REMOVED***
***REMOVED***
