using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class NutritionPlanRequest
    {
        public int NutritionPlanRequestId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanRequestName { get; set; }
        public string? NutritionPlanRequestDescription { get; set; }
        public DateTime? NutritionPlanDateRequested { get; set; }
        public int? NutritionPlanId { get; set; }

        [ForeignKey("NutritionPlanId")]
        public NutritionPlan? NutritionPlan { get; set; }

        public Client? Client { get; set; }
    }
}
