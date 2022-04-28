using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    public class Trainer
    ***REMOVED***
        [DisplayName("ID do Treinador")]
        public int TrainerId ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? TrainerFirstName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? TrainerLastName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Ginásio")]
        public Gym? Gym ***REMOVED*** get; set; ***REMOVED***

        [Required]
        public UserAccountModel UserAccountModel ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Foto de Perfil")]
        public Photo? TrainerProfilePhoto ***REMOVED*** get; set; ***REMOVED***

        [JsonIgnore]
        public List<Client>? Clients ***REMOVED*** get; set; ***REMOVED***
        public List<TrainingPlan>? TrainingPlans ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
***REMOVED***
