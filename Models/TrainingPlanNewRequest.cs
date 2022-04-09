using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class TrainingPlanNewRequest
    {
        public int TrainingPlanNewRequestId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]

        [DisplayName("Nome")]
        public string? TrainingPlanNewRequestName { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        public string? TrainingPlanNewRequestDescription { get; set; }
        [DisplayName("Data")]
        public DateTime? TrainingPlanNewRequestDate { get; set; }

        public bool TrainingPlanNewRequestDone { get; set; } = false;

        public Client? Client { get; set; }
    }
}
