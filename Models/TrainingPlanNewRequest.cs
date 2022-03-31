using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int? TrainingPlanId { get; set; }

        [ForeignKey("TrainingPlanId")]
        public TrainingPlan? TrainingPlan { get; set; }

        public Client? Client { get; set; }
    }
}
