using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// Trainer class
    /// </summary>
    public class Trainer
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the Trainer id.
        /// Display name = ID do Treinador
        /// </summary>
        [DisplayName("ID do Treinador")]
        public int TrainerId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Trainer first name.
        /// Display name = Primeiro Nome
        /// </summary>
        [DisplayName("Primeiro Nome")]
        public string? TrainerFirstName ***REMOVED*** get; set; ***REMOVED***
        // <summary>
        /// Gets and Sets the Trainer last name.
        /// Display name = Último Nome
        /// </summary>
        [DisplayName("Último Nome")]
        public string? TrainerLastName ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Trainer associated Gym.
        /// Display name = Ginásio
        /// </summary>
        [DisplayName("Ginásio")]
        public Gym? Gym ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Trainer user account model.
        /// </summary>
        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Trainer profile photo.
        /// </summary>
        [DisplayName("Foto de Perfil")]
        public Photo? TrainerProfilePhoto ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Trainer client list.
        /// </summary>
        [JsonIgnore]
        public List<Client>? Clients ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Trainer training plan list.
        /// </summary>
        public List<TrainingPlan>? TrainingPlans ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
***REMOVED***
