using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class NutritionPlanRequest
    ***REMOVED***
        public int NutritionPlanRequestId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanRequestName ***REMOVED*** get; set; ***REMOVED***
        public string? NutritionPlanRequestDescription ***REMOVED*** get; set; ***REMOVED***
        public DateTime? NutritionPlanDateRequested ***REMOVED*** get; set; ***REMOVED***
        public int? NutritionPlanId ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("NutritionPlanId")]
        public NutritionPlan? NutritionPlan ***REMOVED*** get; set; ***REMOVED***

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
