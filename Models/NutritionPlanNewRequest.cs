using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class NutritionPlanNewRequest
    {
        public int NutritionPlanNewRequestId { get; set; }
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestName { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        public string? NutritionPlanNewRequestDescription { get; set; }
        [DisplayName("Data")]
        public DateTime? NutritionPlanNewRequestDate { get; set; }

        public bool NutritionPlanNewRequestDone { get; set; } = false;

        public Client? Client { get; set; }
    }
}
