using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
***REMOVED***
    public class NutritionPlanNewRequest
    ***REMOVED***
        public int NutritionPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestName ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestDescription ***REMOVED*** get; set; ***REMOVED***
        public DateTime? NutritionPlanNewRequestDate ***REMOVED*** get; set; ***REMOVED***

        public bool NutritionPlanNewRequestDone ***REMOVED*** get; set; ***REMOVED*** = false;

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
