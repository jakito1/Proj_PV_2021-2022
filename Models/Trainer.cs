using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    public class Trainer
    {
        [DisplayName("ID do Treinador")]
        public int TrainerId { get; set; }

        [DisplayName("Primeiro Nome")]
        public string? TrainerFirstName { get; set; }

        [DisplayName("Último Nome")]
        public string? TrainerLastName { get; set; }

        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        public UserAccountModel? UserAccountModel { get; set; }

        [DisplayName("Foto de Perfil")]
        public Photo? TrainerProfilePhoto { get; set; }

        [JsonIgnore]
        public List<Client>? Clients { get; set; }
        public List<TrainingPlan>? TrainingPlans { get; set; }

    }
}
