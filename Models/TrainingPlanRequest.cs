using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class TrainingPlanRequest
    {
        public int TrainingPlanRequestId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanRequestName { get; set; }
        public string? TrainingPlanRequestDescription { get; set; }
        public DateTime? TrainingPlanDateRequested { get; set; }

        public int? TrainingPlanId { get; set; }

        [ForeignKey("TrainingPlanId")]
        public TrainingPlan? TrainingPlan { get; set; }

        public Client? Client { get; set; }
    }
}
