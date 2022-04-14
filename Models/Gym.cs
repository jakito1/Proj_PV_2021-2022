using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// Gym class
    /// </summary>
    public class Gym
    {
        /// <summary>
        /// Gets and Sets the gym id.
        /// Display name = ID do Ginásio
        /// </summary>
        [DisplayName("ID do Ginásio")]
        public int GymId { get; set; }

        /// <summary>
        /// Gets and Sets the gym name.
        /// Display name = Nome
        /// </summary>
        [DisplayName("Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? GymName { get; set; }
        
        /// <summary>
        /// Gets and Sets the gym profile picture.
        /// Display name = Foto de Perfil
        /// </summary>
        [DisplayName("Foto de Perfil")]
        public Photo? GymProfilePhoto { get; set; }

        /// <summary>
        /// Gets and Sets the gym user account model.
        /// </summary>
        public UserAccountModel? UserAccountModel { get; set; }

        /// <summary>
        /// Gets and Sets the gym client list.
        /// </summary>
        public List<Client>? Clients { get; set; }
        /// <summary>
        /// Gets and Sets the gym nutritionist list.
        /// </summary>
        public List<Nutritionist>? Nutritionists { get; set; }
        /// <summary>
        /// Gets and Sets the gym trainer list.
        /// </summary>
        public List<Trainer>? Trainers { get; set; }
        /// <summary>
        /// Gets and Sets the gym machines list.
        /// </summary>
        public List<Machine>? Machines { get; set; }

    }
}
