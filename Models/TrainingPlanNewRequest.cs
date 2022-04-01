using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class TrainingPlanNewRequest
    {
        public int TrainingPlanNewRequestId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanNewRequestName { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanNewRequestDescription { get; set; }
        public DateTime? TrainingPlanNewRequestDate { get; set; }

        public bool TrainingPlanNewRequestDone { get; set; } = false;

        public Client? Client { get; set; }
    }
}
