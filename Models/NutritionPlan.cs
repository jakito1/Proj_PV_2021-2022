using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class NutritionPlan
    {
        public int NutritionPlanId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionPlanName { get; set; }
        public string? NutritionPlanDescription { get; set; }

        public List<Meal>? Meals { get; set; }
        public Nutritionist? Nutritionist { get; set; }
        public Client? Client { get; set; }

        public int? NutritionPlanNewRequestId { get; set; }

        [ForeignKey("NutritionPlanNewRequestId")]
        public NutritionPlanNewRequest? NutritionPlanNewRequest { get; set; }

        public bool ToBeEdited { get; set; } = false;

        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "NutritionPlans")]
        public string? ClientEmail { get; set; }

        [NotMapped]
        public Meal? Meal { get; set; }
    }
}
