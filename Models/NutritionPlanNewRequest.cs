using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// NutritionPlanNewRequest class
    /// </summary>
    public class NutritionPlanNewRequest
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the new request id.
        /// </summary>
        public int NutritionPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the new request name.
        /// Display name = Nome
        /// </summary>
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanNewRequestName ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the new request description.
        /// Display name = Descrição
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        public string? NutritionPlanNewRequestDescription ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the new request date.
        /// </summary>
        [DisplayName("Data")]
        public DateTime? NutritionPlanNewRequestDate ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Flag to know if the request has been done.
        /// </summary>
        public bool NutritionPlanNewRequestDone ***REMOVED*** get; set; ***REMOVED*** = false;
        /// <summary>
        /// Gets and Sets the Client associated with the request.
        /// </summary>
        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
