using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    public class Client
    ***REMOVED***

        [DisplayName("ID do Cliente")]
        public int ClientId ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ClientFirstName ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ClientLastName ***REMOVED*** get; set; ***REMOVED***


        [DisplayName("Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? ClientBirthday ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Peso")]
        public double? Weight ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Altura")]
        public double? Height ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Massa Magra")]
        [Range(0, 100)]
        public double? LeanMass ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Massa Gorda")]
        [Range(0, 100)]
        public double? FatMass ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Outros Dados")]
        public string? OtherClientData ***REMOVED*** get; set; ***REMOVED***

        public DateTime? DateAddedToGym ***REMOVED*** get; set; ***REMOVED***
        public DateTime? DateAddedToTrainer ***REMOVED*** get; set; ***REMOVED***
        public DateTime? DateAddedToNutritionist ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Ginásio")]
        public Gym? Gym ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Foto de Perfil")]
        public Photo? ClientProfilePhoto ***REMOVED*** get; set; ***REMOVED***

        [DisplayName("Nutricionista")]
        public Nutritionist? Nutritionist ***REMOVED*** get; set; ***REMOVED***
        public bool WantsNutritionist ***REMOVED*** get; set; ***REMOVED*** = false;

        [DisplayName("Treinador")]
        public Trainer? Trainer ***REMOVED*** get; set; ***REMOVED***

        public bool WantsTrainer ***REMOVED*** get; set; ***REMOVED*** = false;

        [Column(TypeName = "nvarchar(24)")]
        [DisplayName("Sexo")]
        public ClientSex? ClientSex ***REMOVED*** get; set; ***REMOVED***

        public List<TrainingPlan>? TrainingPlans ***REMOVED*** get; set; ***REMOVED***
        public List<TrainingPlanNewRequest>? TrainingPlanRequests ***REMOVED*** get; set; ***REMOVED***
        public List<NutritionPlan>? NutritionPlans ***REMOVED*** get; set; ***REMOVED***
        public List<NutritionPlanNewRequest>? NutritionPlanRequests ***REMOVED*** get; set; ***REMOVED***
        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
    public enum ClientSex
    ***REMOVED***
        [Display(Name = "Nenhum")]
        NONE,
        [Display(Name = "Masculino")]
        MALE,
        [Display(Name = "Feminino")]
        FEMALE
***REMOVED***
***REMOVED***
