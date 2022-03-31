using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class NutritionPlanNewRequest
    {
        public int NutritionPlanNewRequestId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestName { get; set; }
        public string? NutritionPlanNewRequestDescription { get; set; }
        public DateTime? NutritionPlanNewRequestDate { get; set; }
        public int? NutritionPlanId { get; set; }

        [ForeignKey("NutritionPlanId")]
        public NutritionPlan? NutritionPlan { get; set; }

        public Client? Client { get; set; }
    }
}
