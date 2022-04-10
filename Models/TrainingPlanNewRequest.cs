using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// TrainingPlanNewRequest class
    /// </summary>
    public class TrainingPlanNewRequest
    {
        /// <summary>
        /// Gets and Sets the new request id.
        /// </summary>
        public int TrainingPlanNewRequestId { get; set; }
        /// <summary>
        /// Gets and Sets the new request name.
        /// Display name = Nome
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Nome")]
        public string? TrainingPlanNewRequestName { get; set; }
        /// <summary>
        /// Gets and Sets the new request description.
        /// Display name = Descrição
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        public string? TrainingPlanNewRequestDescription { get; set; }
        /// <summary>
        /// Gets and Sets the new request date.
        /// </summary>
        [DisplayName("Data")]
        public DateTime? TrainingPlanNewRequestDate { get; set; }
        /// <summary>
        /// Flag to know if the request has been done.
        /// </summary>
        public bool TrainingPlanNewRequestDone { get; set; } = false;
        /// <summary>
        /// Gets and Sets the Client associated with the request.
        /// </summary>
        public Client? Client { get; set; }
    }
}
