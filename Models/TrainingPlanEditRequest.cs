using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// TrainingPlanEditRequest class
    /// </summary>
    public class TrainingPlanEditRequest
    {
        /// <summary>
        /// Gets and Sets the edit request id.
        /// </summary>
        public int TrainingPlanEditRequestId { get; set; }
        /// <summary>
        /// Gets and Sets the edit request description.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? TrainingPlanEditRequestDescription { get; set; }
        /// <summary>
        /// Gets and Sets the edit request date.
        /// </summary>
        public DateTime? TrainingPlanEditRequestDate { get; set; }
        /// <summary>
        /// Gets and Sets the Training Plan id referencing the request.
        /// </summary>
        public int? TrainingPlanId { get; set; }
        /// <summary>
        /// Gets and Sets the Training Plan referenced by the request.
        /// </summary>
        [ForeignKey("TrainingPlanId")]
        public TrainingPlan? TrainingPlan { get; set; }
        /// <summary>
        /// Flag to know it the edit request is completed.
        /// </summary>
        public bool TrainingPlanEditRequestDone { get; set; } = false;
        /// <summary>
        /// Gets and Sets the Client associated with the request.
        /// </summary>
        public Client? Client { get; set; }
    }
}
