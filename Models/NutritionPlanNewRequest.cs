using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class NutritionPlanNewRequest
    ***REMOVED***
        public int NutritionPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestName ***REMOVED*** get; set; ***REMOVED***
        public string? NutritionPlanNewRequestDescription ***REMOVED*** get; set; ***REMOVED***
        public DateTime? NutritionPlanNewRequestDate ***REMOVED*** get; set; ***REMOVED***
        public int? NutritionPlanId ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("NutritionPlanId")]
        public NutritionPlan? NutritionPlan ***REMOVED*** get; set; ***REMOVED***

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
