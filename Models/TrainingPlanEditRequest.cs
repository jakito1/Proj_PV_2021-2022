using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class TrainingPlanEditRequest
    {
        public int TrainingPlanEditRequestId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanEditRequestDescription { get; set; }
        public DateTime? TrainingPlanEditRequestDate { get; set; }

        public int? TrainingPlanId { get; set; }

        [ForeignKey("TrainingPlanId")]
        public TrainingPlan? TrainingPlan { get; set; }

        public Client? Client { get; set; }
    }
}
