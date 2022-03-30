using System.ComponentModel;

namespace NutriFitWeb.Models
{
    public class Gym
    {
        [DisplayName("ID do Ginásio")]
        public int GymId { get; set; }

        [DisplayName("Nome")]
        public string? GymName { get; set; }

        [DisplayName("Foto de Perfil")]
        public Photo? GymProfilePhoto { get; set; }

        public UserAccountModel? UserAccountModel { get; set; }

        public List<Client>? Clients { get; set; }
        public List<Nutritionist>? Nutritionists { get; set; }
        public List<Trainer>? Trainers { get; set; }

    }
}
