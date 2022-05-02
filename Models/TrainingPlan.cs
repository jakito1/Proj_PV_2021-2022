using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// TrainingPlan class
    /// </summary>
    public class TrainingPlan
    {
        /// <summary>
        /// Gets and Sets the Training plan id.
        /// </summary>
        public int TrainingPlanId { get; set; }
        /// <summary>
        /// Gets and Sets the Training plan name.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? TrainingPlanName { get; set; }
        /// <summary>
        /// Gets and Sets the Training plan description.
        /// </summary>
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? TrainingPlanDescription { get; set; }
        /// <summary>
        /// Gets and Sets the Exercise list of the training plan.
        /// </summary>
        public List<Exercise>? Exercises { get; set; }
        /// <summary>
        /// Gets and Sets the Trainer associated with the plan.
        /// </summary>
        public Trainer? Trainer { get; set; }
        /// <summary>
        /// Gets and Sets the Client associated with the plan.
        /// </summary>
        public Client? Client { get; set; }
        /// <summary>
        /// Gets and Sets the request id for the training plan.
        /// </summary>
        public int? TrainingPlanNewRequestId { get; set; }
        /// <summary>
        /// Gets and Sets the request associated with the plan.
        /// </summary>
        [ForeignKey("TrainingPlanNewRequestId")]
        public TrainingPlanNewRequest? TrainingPlanNewRequest { get; set; }
        /// <summary>
        /// Flag to know if the plan has to be edited.
        /// </summary>
        public bool ToBeEdited { get; set; } = false;
        /// <summary>
        /// Gets and Sets the client's email.
        /// </summary>
        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "TrainingPlans")]
        public string? ClientEmail { get; set; }
        /// <summary>
        /// Gets and Sets the edit request associated with the plan.
        /// </summary>
        [NotMapped]
        public TrainingPlanEditRequest? TrainingPlanEditRequest { get; set; }
        /// <summary>
        /// Gets and Sets the Exercise associated with the plan.
        /// </summary>
        [NotMapped]
        public Exercise? Exercise { get; set; }
    }

}
