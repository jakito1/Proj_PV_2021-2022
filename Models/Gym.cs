using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class Gym
    {
        [DisplayName("ID do Ginásio")]
        public int GymId { get; set; }

        [DisplayName("Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? GymName { get; set; }

        [DisplayName("Foto de Perfil")]
        [RegularExpression(@"(.*\.)(jpe?g|gif)$", ErrorMessage = "Apenas imagens são permitidas.")]
        public Photo? GymProfilePhoto { get; set; }

        public UserAccountModel UserAccountModel { get; set; }

        public List<Client>? Clients { get; set; }
        public List<Nutritionist>? Nutritionists { get; set; }
        public List<Trainer>? Trainers { get; set; }
        public List<Machine>? Machines { get; set; }

    }
}
