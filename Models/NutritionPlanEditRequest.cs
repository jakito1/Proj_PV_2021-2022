using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// NutritionPlanEditRequest class
    /// </summary>
    public class NutritionPlanEditRequest
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the edit request id.
        /// </summary>
        public int NutritionPlanEditRequestId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the edit request description.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? NutritionPlanEditRequestDescription ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the edit request date.
        /// </summary>
        public DateTime? NutritionPlanEditRequestDate ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutrition Plan id referencing the request.
        /// </summary>
        public int? NutritionPlanId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutrition Plan referenced by the request.
        /// </summary>
        [ForeignKey("NutritionPlanId")]
        public NutritionPlan? NutritionPlan ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Flag to know it the edit request is completed.
        /// </summary>
        public bool NutritionPlanEditRequestDone ***REMOVED*** get; set; ***REMOVED*** = false;
        /// <summary>
        /// Gets and Sets the Client associated with the request.
        /// </summary>
        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
