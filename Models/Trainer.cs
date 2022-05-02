using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// Trainer class
    /// </summary>
    public class Trainer
    {
        /// <summary>
        /// Gets and Sets the Trainer id.
        /// Display name = ID do Treinador
        /// </summary>
        [DisplayName("ID do Treinador")]
        public int TrainerId { get; set; }
        /// <summary>
        /// Gets and Sets the Trainer first name.
        /// Display name = Primeiro Nome
        /// </summary>
        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? TrainerFirstName { get; set; }
        // <summary>
        /// Gets and Sets the Trainer last name.
        /// Display name = Último Nome
        /// </summary>
        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? TrainerLastName { get; set; }
        /// <summary>
        /// Gets and Sets the Trainer associated Gym.
        /// Display name = Ginásio
        /// </summary>
        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        public UserAccountModel UserAccountModel { get; set; }

        [DisplayName("Foto de Perfil")]
        public Photo? TrainerProfilePhoto { get; set; }
        /// <summary>
        /// Gets and Sets the Trainer client list.
        /// </summary>
        [JsonIgnore]
        public List<Client>? Clients { get; set; }
        /// <summary>
        /// Gets and Sets the Trainer training plan list.
        /// </summary>
        public List<TrainingPlan>? TrainingPlans { get; set; }

    }
}
