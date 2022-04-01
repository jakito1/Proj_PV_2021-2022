using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class TrainingPlanEditRequest
    ***REMOVED***
        public int TrainingPlanEditRequestId ***REMOVED*** get; set; ***REMOVED***

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanEditRequestDescription ***REMOVED*** get; set; ***REMOVED***
        public DateTime? TrainingPlanEditRequestDate ***REMOVED*** get; set; ***REMOVED***

        public int? TrainingPlanId ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("TrainingPlanId")]
        public TrainingPlan? TrainingPlan ***REMOVED*** get; set; ***REMOVED***

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
