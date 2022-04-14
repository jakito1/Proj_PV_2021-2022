using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// TrainingPlan class
    /// </summary>
    public class TrainingPlan
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the Training plan id.
        /// </summary>
        public int TrainingPlanId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Training plan name.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string? TrainingPlanName ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Training plan description.
        /// </summary>
        public string? TrainingPlanDescription ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Exercise list of the training plan.
        /// </summary>
        public List<Exercise>? Exercises ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Trainer associated with the plan.
        /// </summary>
        public Trainer? Trainer ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Client associated with the plan.
        /// </summary>
        public Client? Client ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the request id for the training plan.
        /// </summary>
        public int? TrainingPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the request associated with the plan.
        /// </summary>
        [ForeignKey("TrainingPlanNewRequestId")]
        public TrainingPlanNewRequest? TrainingPlanNewRequest ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Flag to know if the plan has to be edited.
        /// </summary>
        public bool ToBeEdited ***REMOVED*** get; set; ***REMOVED*** = false;
        /// <summary>
        /// Gets and Sets the client's email.
        /// </summary>
        [NotMapped]
        [Remote(action: "VerifyClientEmail", controller: "TrainingPlans")]
        public string? ClientEmail ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the edit request associated with the plan.
        /// </summary>
        [NotMapped]
        public TrainingPlanEditRequest? TrainingPlanEditRequest ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Exercise associated with the plan.
        /// </summary>
        [NotMapped]
        public Exercise? Exercise ***REMOVED*** get; set; ***REMOVED***
***REMOVED***

***REMOVED***
