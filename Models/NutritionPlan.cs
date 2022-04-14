using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// NutritionPlan class
    /// </summary>
    public class NutritionPlan
    {
        /// <summary>
        /// Gets and Sets the Nutrition plan id.
        /// </summary>
        public int NutritionPlanId { get; set; }
        /// <summary>
        /// Gets and Sets the Nutrition plan name.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanName { get; set; }
        /// <summary>
        /// Gets and Sets the Nutrition plan description.
        /// </summary>
        public string? NutritionPlanDescription { get; set; }
        /// <summary>
        /// Gets and Sets the Meal list of the nutrition plan.
        /// </summary>
        public List<Meal>? Meals { get; set; }
        /// <summary>
        /// Gets and Sets the Nutritionist associated with the plan.
        /// </summary>
        public Nutritionist? Nutritionist { get; set; }
        /// <summary>
        /// Gets and Sets the Client associated with the plan.
        /// </summary>
        public Client? Client { get; set; }
        /// <summary>
        /// Gets and Sets the request id for the nutrition plan.
        /// </summary>
        public int? NutritionPlanNewRequestId { get; set; }
        /// <summary>
        /// Gets and Sets the request associated with the plan.
        /// </summary>
        [ForeignKey("NutritionPlanNewRequestId")]
        public NutritionPlanNewRequest? NutritionPlanNewRequest { get; set; }
        /// <summary>
        /// Flag to know if the plan has to be edited.
        /// </summary>
        public bool ToBeEdited { get; set; } = false;
        /// <summary>
        /// Gets and Sets the client's email.
        /// </summary>
        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "NutritionPlans")]
        public string? ClientEmail { get; set; }
        /// <summary>
        /// Gets and Sets the Meal associated with the plan.
        /// </summary>
        [NotMapped]
        public Meal? Meal { get; set; }
    }
}
