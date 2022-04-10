using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class NutritionPlanNewRequest
    {
        public int NutritionPlanNewRequestId { get; set; }
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionPlanNewRequestName { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? NutritionPlanNewRequestDescription { get; set; }
        [DisplayName("Data")]
        public DateTime? NutritionPlanNewRequestDate { get; set; }

        public bool NutritionPlanNewRequestDone { get; set; } = false;

        public Client? Client { get; set; }
    }
}
