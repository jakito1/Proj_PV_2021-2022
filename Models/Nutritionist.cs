using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// Nutritionist class
    /// </summary>
    public class Nutritionist
    {
        /// <summary>
        /// Gets and Sets the Nutritionist id.
        /// Display name = ID do Nutricionista
        /// </summary>
        [DisplayName("ID do Nutricionista")]
        public int NutritionistId { get; set; }
        /// <summary>
        /// Gets and Sets the Nutritionist first name.
        /// Display name = Primeiro Nome
        /// </summary>
        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionistFirstName { get; set; }
        // <summary>
        /// Gets and Sets the Nutritionist last name.
        /// Display name = Último Nome
        /// </summary>
        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionistLastName { get; set; }
        /// <summary>
        /// Gets and Sets the Nutritionist associated Gym.
        /// Display name = Ginásio
        /// </summary>
        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        public UserAccountModel UserAccountModel { get; set; }
        [DisplayName("Foto de Perfil")]
        public Photo? NutritionistProfilePhoto { get; set; }
        /// <summary>
        /// Gets and Sets the Nutritionist client list.
        /// </summary>
        [JsonIgnore]
        public List<Client>? Clients { get; set; }
        /// <summary>
        /// Gets and Sets the Nutritionist nutrition plan list.
        /// </summary>
        public List<NutritionPlan>? NutritionPlans { get; set; }

    }
}
