using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class NutritionPlanEditRequest
    ***REMOVED***
        public int NutritionPlanEditRequestId ***REMOVED*** get; set; ***REMOVED***

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? NutritionPlanEditRequestDescription ***REMOVED*** get; set; ***REMOVED***
        public DateTime? NutritionPlanEditRequestDate ***REMOVED*** get; set; ***REMOVED***

        public int? NutritionPlanId ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("NutritionPlanId")]
        public NutritionPlan? NutritionPlan ***REMOVED*** get; set; ***REMOVED***

        public bool NutritionPlanEditRequestDone ***REMOVED*** get; set; ***REMOVED*** = false;

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
