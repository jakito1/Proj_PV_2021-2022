using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class NutritionPlan
    ***REMOVED***
        public int NutritionPlanId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionPlanName ***REMOVED*** get; set; ***REMOVED***
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? NutritionPlanDescription ***REMOVED*** get; set; ***REMOVED***

        public List<Meal>? Meals ***REMOVED*** get; set; ***REMOVED***
        public Nutritionist? Nutritionist ***REMOVED*** get; set; ***REMOVED***
        public Client? Client ***REMOVED*** get; set; ***REMOVED***

        public int? NutritionPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***

        [ForeignKey("NutritionPlanNewRequestId")]
        public NutritionPlanNewRequest? NutritionPlanNewRequest ***REMOVED*** get; set; ***REMOVED***

        public bool ToBeEdited ***REMOVED*** get; set; ***REMOVED*** = false;

        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "NutritionPlans")]
        public string? ClientEmail ***REMOVED*** get; set; ***REMOVED***

        [NotMapped]
        public Meal? Meal ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
