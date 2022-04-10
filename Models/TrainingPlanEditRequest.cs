using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// TrainingPlanEditRequest class
    /// </summary>
    public class TrainingPlanEditRequest
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the edit request id.
        /// </summary>
        public int TrainingPlanEditRequestId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the edit request description.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(1000, ErrorMessage = "A descrição não pode exceder 1000 caracteres.")]
        public string? TrainingPlanEditRequestDescription ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the edit request date.
        /// </summary>
        public DateTime? TrainingPlanEditRequestDate ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Training Plan id referencing the request.
        /// </summary>
        public int? TrainingPlanId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Training Plan referenced by the request.
        /// </summary>
        [ForeignKey("TrainingPlanId")]
        public TrainingPlan? TrainingPlan ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Flag to know it the edit request is completed.
        /// </summary>
        public bool TrainingPlanEditRequestDone ***REMOVED*** get; set; ***REMOVED*** = false;
        /// <summary>
        /// Gets and Sets the Client associated with the request.
        /// </summary>
        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
