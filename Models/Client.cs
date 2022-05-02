using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitWeb.Models
{
    /// <summary>
    /// Client class
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Gets and Sets the Client Id.
        /// Display name = ID do Cliente
        /// </summary>
        [DisplayName("ID do Cliente")]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets and Sets the Client FirstName.
        /// Display name = Primeiro Nome
        /// </summary>
        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ClientFirstName { get; set; }

        /// <summary>
        /// Gets and Sets the Client LastName.
        /// Display name = Último Nome
        /// </summary>
        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? ClientLastName { get; set; }

        /// <summary>
        /// Gets and Sets the Client DoB
        /// Display name = Data de Nascimento
        /// </summary>
        [DisplayName("Data de Nascimento")]
        [DataType(DataType.Date)]
        [Remote(action: "VerifyClientAge", controller: "Clients")]
        public DateTime? ClientBirthday { get; set; }

        /// <summary>
        /// Gets and Sets the Client weight.
        /// Display name = Peso
        /// </summary>
        [DisplayName("Peso")]
        [Range(0.0, 999.9, ErrorMessage = "Indique um valor entre {1} e {2} quilogramas.")]
        public double? Weight { get; set; }

        /// <summary>
        /// Gets and Sets the Client height.
        /// Display name = Altura
        /// </summary>
        [DisplayName("Altura")]
        [Range(1, 999, ErrorMessage = "Indique um valor inteiro entre {1} e {2} centímetros.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Deve inserir um valor inteiro.")]
        public int? Height { get; set; }

        /// <summary>
        /// Gets and Sets the Client lean mass.
        /// Display name = Massa Magra
        /// </summary>
        [DisplayName("Massa Magra")]
        [Range(0.1, 100.0, ErrorMessage = "Indique um valor inteiro entre {1} e {2}.")]
        public double? LeanMass { get; set; }

        /// <summary>
        /// Gets and Sets the Client fat mass.
        /// Display name = Massa Gorda
        /// </summary>
        [DisplayName("Massa Gorda")]
        [Range(0.1, 100.0, ErrorMessage = "Indique um valor inteiro entre {1} e {2}.")]
        public double? FatMass { get; set; }

        /// <summary>
        /// Gets and Sets other type of Client data.
        /// Display name = Outros Dados
        /// </summary>
        [DisplayName("Outros Dados")]
        public string? OtherClientData { get; set; }

        public DateTime? DateAddedToGym { get; set; }
        public DateTime? DateAddedToTrainer { get; set; }
        public DateTime? DateAddedToNutritionist { get; set; }

        /// <summary>
        /// Gets and Sets the Gym associated to the client.
        /// Display name = Ginásio
        /// </summary>
        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        /// <summary>
        /// Gets and Sets the Client profile picture.
        /// Display name = Foto de Perfil
        /// </summary>
        [DisplayName("Foto de Perfil")]
        public Photo? ClientProfilePhoto { get; set; }

        /// <summary>
        /// Gets and Sets the Nutritionist associated to the client.
        /// Display name = Nutricionista
        /// </summary>
        [DisplayName("Nutricionista")]
        public Nutritionist? Nutritionist { get; set; }

        /// <summary>
        /// Flag to check if the client wants a Nutritionist.
        /// </summary>
        public bool WantsNutritionist { get; set; } = false;

        /// <summary>
        /// Gets and Sets the Trainer associated with the client.
        /// Display name = Treinador
        /// </summary>
        [DisplayName("Treinador")]
        public Trainer? Trainer { get; set; }

        /// <summary>
        /// Flag to check if the client wants a Trainer.
        /// </summary>
        public bool WantsTrainer { get; set; } = false;

        [Column(TypeName = "nvarchar(24)")]
        [DisplayName("Sexo")]
        public ClientSex? ClientSex { get; set; }

        /// <summary>
        /// Gets and Sets the training plan list of the client.
        /// </summary>
        public List<TrainingPlan>? TrainingPlans { get; set; }
        /// <summary>
        /// Gets and Sets the training plan request list of the client.
        /// </summary>
        public List<TrainingPlanNewRequest>? TrainingPlanRequests { get; set; }
        /// <summary>
        /// Gets and Sets the nutrition plans list of the client.
        /// </summary>
        public List<NutritionPlan>? NutritionPlans { get; set; }
        /// <summary>
        /// Gets and Sets the nutrition plan request list of the client.
        /// </summary>
        public List<NutritionPlanNewRequest>? NutritionPlanRequests { get; set; }

        public UserAccountModel UserAccountModel { get; set; }

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
