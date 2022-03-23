using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class TrainingPlan
    ***REMOVED***
        public int TrainingPlanId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanName ***REMOVED*** get; set; ***REMOVED***
        public string? TrainingPlanDescription ***REMOVED*** get; set; ***REMOVED***

        public List<Exercise>? Exercises ***REMOVED*** get; set; ***REMOVED***
        public Trainer? Trainer ***REMOVED*** get; set; ***REMOVED***
        public Client? Client ***REMOVED*** get; set; ***REMOVED***

        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "TrainingPlans")]
        public string? ClientEmail ***REMOVED*** get; set; ***REMOVED***

        [NotMapped]
        public Exercise? Exercise ***REMOVED*** get; set; ***REMOVED***
***REMOVED***

***REMOVED***
