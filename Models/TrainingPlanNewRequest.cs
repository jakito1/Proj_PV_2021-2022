using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
***REMOVED***
    public class TrainingPlanNewRequest
    ***REMOVED***
        public int TrainingPlanNewRequestId ***REMOVED*** get; set; ***REMOVED***
        [Required(ErrorMessage = "Campo Obrigatório")]

        [DisplayName("Nome")]
        public string? TrainingPlanNewRequestName ***REMOVED*** get; set; ***REMOVED***

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Descrição")]
        public string? TrainingPlanNewRequestDescription ***REMOVED*** get; set; ***REMOVED***
        [DisplayName("Data")]
        public DateTime? TrainingPlanNewRequestDate ***REMOVED*** get; set; ***REMOVED***

        public bool TrainingPlanNewRequestDone ***REMOVED*** get; set; ***REMOVED*** = false;

        public Client? Client ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
