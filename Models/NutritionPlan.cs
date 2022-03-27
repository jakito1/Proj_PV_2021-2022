using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class NutritionPlan
    {
        public int NutritionPlanId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanName { get; set; }
        public string? NutritionPlanDescription { get; set; }

        public List<Meal>? Meals { get; set; }
        public Nutritionist? Nutritionist { get; set; }
        public Client? Client { get; set; }

        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "NutritionPlans")]
        public string? ClientEmail { get; set; }

        [NotMapped]
        public Meal? Meal { get; set; }
    }
}
