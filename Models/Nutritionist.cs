using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    public class Nutritionist
    {
        [DisplayName("ID do Nutricionista")]
        public int NutritionistId { get; set; }

        [DisplayName("Primeiro Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionistFirstName { get; set; }

        [DisplayName("Último Nome")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string? NutritionistLastName { get; set; }

        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        public UserAccountModel? UserAccountModel { get; set; }
        [DisplayName("Foto de Perfil")]
        public Photo? NutritionistProfilePhoto { get; set; }

        [JsonIgnore]
        public List<Client>? Clients { get; set; }
        public List<NutritionPlan>? NutritionPlans { get; set; }

    }
}
