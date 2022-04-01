using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class NutritionPlanNewRequest
    {
        public int NutritionPlanNewRequestId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestName { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestDescription { get; set; }
        public DateTime? NutritionPlanNewRequestDate { get; set; }

        public bool NutritionPlanNewRequestDone { get; set; } = false;

        public Client? Client { get; set; }
    }
}
