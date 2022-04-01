using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class TrainingPlanNewRequest
    ***REMOVED***
        public int TrainingPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanNewRequestName ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanNewRequestDescription ***REMOVED*** get; set; ***REMOVED***
        public DateTime? TrainingPlanNewRequestDate ***REMOVED*** get; set; ***REMOVED***

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
