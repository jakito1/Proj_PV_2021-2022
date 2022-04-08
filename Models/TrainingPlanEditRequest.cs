using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class TrainingPlanEditRequest
    {
        public int TrainingPlanEditRequestId { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? TrainingPlanEditRequestDescription { get; set; }
        public DateTime? TrainingPlanEditRequestDate { get; set; }

        public int? TrainingPlanId { get; set; }

        [ForeignKey("TrainingPlanId")]
        public TrainingPlan? TrainingPlan { get; set; }

        public bool TrainingPlanEditRequestDone { get; set; } = false;

        public Client? Client { get; set; }
    }
}
