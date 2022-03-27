using System.ComponentModel;
using System.Text.Json.Serialization;

namespace NutriFitWeb.Models
{
    public class Nutritionist
    {
        [DisplayName("ID do Nutricionista")]
        public int NutritionistId { get; set; }

        [DisplayName("Primeiro Nome")]
        public string? NutritionistFirstName { get; set; }

        [DisplayName("Último Nome")]
        public string? NutritionistLastName { get; set; }

        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        public UserAccountModel? UserAccountModel { get; set; }

        [JsonIgnore]
        public List<Client>? Clients { get; set; }
        public List<NutritionPlan>? NutritionPlans { get; set; }

    }
}
