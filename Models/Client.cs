using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class Client
    {

        [DisplayName("ID do Cliente")]
        public int ClientId { get; set; }

        [DisplayName("Primeiro Nome")]
        public string? ClientFirstName { get; set; }

        [DisplayName("Último Nome")]
        public string? ClientLastName { get; set; }


        [DisplayName("Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? ClientBirthday { get; set; }

        [DisplayName("Peso")]
        public double? Weight { get; set; }

        [DisplayName("Altura")]
        public double? Height { get; set; }

        [DisplayName("Massa Magra")]
        [Range(0, 100)]
        public double? LeanMass { get; set; }

        [DisplayName("Massa Gorda")]
        [Range(0, 100)]
        public double? FatMass { get; set; }

        [DisplayName("Outros Dados")]
        public string? OtherClientData { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        [DisplayName("Foto de Perfil")]
        public Photo? ClientProfilePhoto { get; set; }

        [DisplayName("Nutricionista")]
        public Nutritionist? Nutritionist { get; set; }
        public bool WantsNutritionist { get; set; } = false;

        [DisplayName("Treinador")]
        public Trainer? Trainer { get; set; }

        public bool WantsTrainer { get; set; } = false;

        public List<TrainingPlan>? TrainingPlans { get; set; }
        public List<TrainingPlanNewRequest>? TrainingPlanRequests { get; set; }
        public List<NutritionPlan>? NutritionPlans { get; set; }
        public List<NutritionPlanNewRequest>? NutritionPlanRequests { get; set; }


        public UserAccountModel? UserAccountModel { get; set; }
    }
}
