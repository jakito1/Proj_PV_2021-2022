using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// NutritionPlanNewRequest class
    /// </summary>
    public class NutritionPlanNewRequest
    {
        /// <summary>
        /// Gets and Sets the new request id.
        /// </summary>
        public int NutritionPlanNewRequestId { get; set; }
        /// <summary>
        /// Gets and Sets the new request name.
        /// Display name = Nome
        /// </summary>
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestName { get; set; }
        /// <summary>
        /// Gets and Sets the new request description.
        /// Display name = Descrição
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        public string? NutritionPlanNewRequestDescription { get; set; }
        /// <summary>
        /// Gets and Sets the new request date.
        /// </summary>
        [DisplayName("Data")]
        public DateTime? NutritionPlanNewRequestDate { get; set; }
        /// <summary>
        /// Flag to know if the request has been done.
        /// </summary>
        public bool NutritionPlanNewRequestDone { get; set; } = false;
        /// <summary>
        /// Gets and Sets the Client associated with the request.
        /// </summary>
        public Client? Client { get; set; }
    }
}
