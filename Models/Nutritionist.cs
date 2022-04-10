using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
***REMOVED***
    /// <summary>
    /// Nutritionist class
    /// </summary>
    public class Nutritionist
    ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutritionist id.
        /// Display name = ID do Nutricionista
        /// </summary>
        [DisplayName("ID do Nutricionista")]
        public int NutritionistId ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutritionist first name.
        /// Display name = Primeiro Nome
        /// </summary>
        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionistFirstName ***REMOVED*** get; set; ***REMOVED***
        // <summary>
        /// Gets and Sets the Nutritionist last name.
        /// Display name = Último Nome
        /// </summary>
        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionistLastName ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutritionist associated Gym.
        /// Display name = Ginásio
        /// </summary>
        [DisplayName("Ginásio")]
        public Gym? Gym ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutritionist user account model.
        /// </summary>
        public UserAccountModel? UserAccountModel ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutritionist profile photo.
        /// </summary>
        [DisplayName("Foto de Perfil")]
        public Photo? NutritionistProfilePhoto ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutritionist client list.
        /// </summary>
        [JsonIgnore]
        public List<Client>? Clients ***REMOVED*** get; set; ***REMOVED***
        /// <summary>
        /// Gets and Sets the Nutritionist nutrition plan list.
        /// </summary>
        public List<NutritionPlan>? NutritionPlans ***REMOVED*** get; set; ***REMOVED***

***REMOVED***
***REMOVED***
