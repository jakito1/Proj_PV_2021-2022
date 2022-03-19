using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class TrainingPlan
    ***REMOVED***
        public int TrainingPlanId ***REMOVED*** get; set; ***REMOVED***
        public string? TrainingPlanName ***REMOVED*** get; set; ***REMOVED***
        public string? TrainingPlanDescription ***REMOVED*** get; set; ***REMOVED***

        public List<Exercise>? Exercises ***REMOVED*** get; set; ***REMOVED***
        public Trainer? Trainer ***REMOVED*** get; set; ***REMOVED***
        public Client? Client ***REMOVED*** get; set; ***REMOVED***

        [NotMapped]
        public string? ClientEmail ***REMOVED*** get; set; ***REMOVED***
***REMOVED***

***REMOVED***
