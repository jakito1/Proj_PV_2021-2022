using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class TrainingPlan
    {
        public int TrainingPlanId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? TrainingPlanName { get; set; }
        public string? TrainingPlanDescription { get; set; }

        public List<Exercise>? Exercises { get; set; }
        public Trainer? Trainer { get; set; }
        public Client? Client { get; set; }

        public int? TrainingPlanNewRequestId { get; set; }

        [ForeignKey("TrainingPlanNewRequestId")]
        public TrainingPlanNewRequest? TrainingPlanNewRequest { get; set; }
        public bool ToBeEdited { get; set; } = false;

        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "TrainingPlans")]
        public string? ClientEmail { get; set; }

        [NotMapped]
        public TrainingPlanEditRequest? TrainingPlanEditRequest { get; set; }

        [NotMapped]
        public Exercise? Exercise { get; set; }
    }

}
