using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// Client class
    /// </summary>
    public class Client
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the Client Id.
        /// Display name = ID do Cliente
        /// </summary>
        [DisplayName("ID do Cliente")]
        public int ClientId ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Client FirstName.
        /// Display name = Primeiro Nome
        /// </summary>
        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ClientFirstName ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Client LastName.
        /// Display name = Último Nome
        /// </summary>
        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ClientLastName ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Client DoB
        /// Display name = Data de Nascimento
        /// </summary>
        [DisplayName("Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? ClientBirthday ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Client weight.
        /// Display name = Peso
        /// </summary>
        [DisplayName("Peso")]
        [Range(0.0, 999.9, ErrorMessage = "Indique um valor entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** quilogramas.")]
        public double? Weight ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Client height.
        /// Display name = Altura
        /// </summary>
        [DisplayName("Altura")]
        [Range(1, 999, ErrorMessage = "Indique um valor inteiro entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED*** centímetros.")]
        public int? Height ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Client lean mass.
        /// Display name = Massa Magra
        /// </summary>
        [DisplayName("Massa Magra")]
        [Range(0.1, 100.0, ErrorMessage = "Indique um valor inteiro entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED***.")]
        public double? LeanMass ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Client fat mass.
        /// Display name = Massa Gorda
        /// </summary>
        [DisplayName("Massa Gorda")]
        [Range(0.1, 100.0, ErrorMessage = "Indique um valor inteiro entre ***REMOVED***1***REMOVED*** e ***REMOVED***2***REMOVED***.")]
        public double? FatMass ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets other type of Client data.
        /// Display name = Outros Dados
        /// </summary>
        [DisplayName("Outros Dados")]
        public string? OtherClientData ***REMOVED*** get; set; ***REMOVED***

        public DateTime? DateAddedToGym ***REMOVED*** get; set; ***REMOVED***
        public DateTime? DateAddedToTrainer ***REMOVED*** get; set; ***REMOVED***
        public DateTime? DateAddedToNutritionist ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Gym associated to the client.
        /// Display name = Ginásio
        /// </summary>
        [DisplayName("Ginásio")]
        public Gym? Gym ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Client profile picture.
        /// Display name = Foto de Perfil
        /// </summary>
        [DisplayName("Foto de Perfil")]
        public Photo? ClientProfilePhoto ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the Nutritionist associated to the client.
        /// Display name = Nutricionista
        /// </summary>
        [DisplayName("Nutricionista")]
        public Nutritionist? Nutritionist ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Flag to check if the client wants a Nutritionist.
        /// </summary>
        public bool WantsNutritionist ***REMOVED*** get; set; ***REMOVED*** = false;

        /// <summary>
        /// Gets and Sets the Trainer associated with the client.
        /// Display name = Treinador
        /// </summary>
        [DisplayName("Treinador")]
        public Trainer? Trainer ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Flag to check if the client wants a Trainer.
        /// </summary>
        public bool WantsTrainer ***REMOVED*** get; set; ***REMOVED*** = false;

        [Column(TypeName = "nvarchar(24)")]
        [DisplayName("Sexo")]
        public ClientSex? ClientSex ***REMOVED*** get; set; ***REMOVED***

        /// <summary>
        /// Gets and Sets the training plan list of the client.
        /// </summary>
        public List<TrainingPlan>? TrainingPlans ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the training plan request list of the client.
        /// </summary>
        public List<TrainingPlanNewRequest>? TrainingPlanRequests ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the nutrition plans list of the client.
        /// </summary>
        public List<NutritionPlan>? NutritionPlans ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the nutrition plan request list of the client.
        /// </summary>
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
        /// <summary>
        /// Gets and Sets the client's user account model.
        /// </summary>
        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***
***REMOVED***
***REMOVED***
