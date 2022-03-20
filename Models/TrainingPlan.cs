using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    public class TrainingPlan
    {
        public int TrainingPlanId { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanName { get; set; }
        public string? TrainingPlanDescription { get; set; }

        public List<Exercise>? Exercises { get; set; }
        public Trainer? Trainer { get; set; }
        public Client? Client { get; set; }

        [NotMapped]
        [Required]
        [Remote(action: "VerifyClientEmail", controller: "Clients")]
        public string? ClientEmail { get; set; }

        [NotMapped]
        public Exercise? Exercise { get; set; }
    }

}
