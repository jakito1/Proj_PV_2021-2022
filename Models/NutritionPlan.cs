using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// NutritionPlan class
    /// </summary>
    public class NutritionPlan
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutrition plan id.
        /// </summary>
        public int NutritionPlanId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutrition plan name.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionPlanName ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutrition plan description.
        /// </summary>
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? NutritionPlanDescription ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Meal list of the nutrition plan.
        /// </summary>
        public List<Meal>? Meals ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutritionist associated with the plan.
        /// </summary>
        public Nutritionist? Nutritionist ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Client associated with the plan.
        /// </summary>
        public Client? Client ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the request id for the nutrition plan.
        /// </summary>
        public int? NutritionPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the request associated with the plan.
        /// </summary>
        [ForeignKey("NutritionPlanNewRequestId")]
        public NutritionPlanNewRequest? NutritionPlanNewRequest ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Flag to know if the plan has to be edited.
        /// </summary>
        public bool ToBeEdited ***REMOVED*** get; set; ***REMOVED*** = false;
        /// <summary>
        /// Gets and Sets the client's email.
        /// </summary>
        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "NutritionPlans")]
        public string? ClientEmail ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Meal associated with the plan.
        /// </summary>
        [NotMapped]
        public Meal? Meal ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
