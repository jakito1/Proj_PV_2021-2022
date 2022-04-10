using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
***REMOVED***
    public class NutritionPlanNewRequest
    ***REMOVED***
        public int NutritionPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionPlanNewRequestName ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? NutritionPlanNewRequestDescription ***REMOVED*** get; set; ***REMOVED***
        [DisplayName("Data")]
        public DateTime? NutritionPlanNewRequestDate ***REMOVED*** get; set; ***REMOVED***

        public bool NutritionPlanNewRequestDone ***REMOVED*** get; set; ***REMOVED*** = false;

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
