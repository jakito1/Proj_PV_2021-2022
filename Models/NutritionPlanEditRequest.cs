using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// NutritionPlanEditRequest class
    /// </summary>
    public class NutritionPlanEditRequest
    {
        /// <summary>
        /// Gets and Sets the edit request id.
        /// </summary>
        public int NutritionPlanEditRequestId { get; set; }
        /// <summary>
        /// Gets and Sets the edit request description.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanEditRequestDescription { get; set; }
        /// <summary>
        /// Gets and Sets the edit request date.
        /// </summary>
        public DateTime? NutritionPlanEditRequestDate { get; set; }
        /// <summary>
        /// Gets and Sets the Nutrition Plan id referencing the request.
        /// </summary>
        public int? NutritionPlanId { get; set; }
        /// <summary>
        /// Gets and Sets the Nutrition Plan referenced by the request.
        /// </summary>
        [ForeignKey("NutritionPlanId")]
        public NutritionPlan? NutritionPlan { get; set; }
        /// <summary>
        /// Flag to know it the edit request is completed.
        /// </summary>
        public bool NutritionPlanEditRequestDone { get; set; } = false;
        /// <summary>
        /// Gets and Sets the Client associated with the request.
        /// </summary>
        public Client? Client { get; set; }
    }
}
