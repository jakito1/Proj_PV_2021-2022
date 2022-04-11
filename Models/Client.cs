using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NutriFitWeb.Models
{
    public class Client
    {

        [DisplayName("ID do Cliente")]
        public int ClientId { get; set; }

        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ClientFirstName { get; set; }

        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
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

        public DateTime? DateAddedToGym { get; set; }

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

        [Column(TypeName = "nvarchar(24)")]
        public ClientSex? ClientSex { get; set; }

        public List<TrainingPlan>? TrainingPlans { get; set; }
        public List<TrainingPlanNewRequest>? TrainingPlanRequests { get; set; }
        public List<NutritionPlan>? NutritionPlans { get; set; }
        public List<NutritionPlanNewRequest>? NutritionPlanRequests { get; set; }
        public UserAccountModel? UserAccountModel { get; set; }

    }
    public enum ClientSex
    {
        [Display(Name = "Nenhum")]
        NONE,
        [Display(Name = "Masculino")]
        MALE,
        [Display(Name = "Feminino")]
        FEMALE
    }
}
