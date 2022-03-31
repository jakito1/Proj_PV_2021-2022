using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class TrainingPlanRequest
    ***REMOVED***
        public int TrainingPlanRequestId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanRequestName ***REMOVED*** get; set; ***REMOVED***
        public string? TrainingPlanRequestDescription ***REMOVED*** get; set; ***REMOVED***
        public DateTime? TrainingPlanDateRequested ***REMOVED*** get; set; ***REMOVED***

        public int? TrainingPlanId ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("TrainingPlanId")]
        public TrainingPlan? TrainingPlan ***REMOVED*** get; set; ***REMOVED***

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
