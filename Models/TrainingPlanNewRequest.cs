using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// TrainingPlanNewRequest class
    /// </summary>
    public class TrainingPlanNewRequest
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the new request id.
        /// </summary>
        public int TrainingPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the new request name.
        /// Display name = Nome
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Nome")]
        public string? TrainingPlanNewRequestName ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the new request description.
        /// Display name = Descrição
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        public string? TrainingPlanNewRequestDescription ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the new request date.
        /// </summary>
        [DisplayName("Data")]
        public DateTime? TrainingPlanNewRequestDate ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Flag to know if the request has been done.
        /// </summary>
        public bool TrainingPlanNewRequestDone ***REMOVED*** get; set; ***REMOVED*** = false;
        /// <summary>
        /// Gets and Sets the Client associated with the request.
        /// </summary>
        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
