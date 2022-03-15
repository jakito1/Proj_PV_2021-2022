using System.ComponentModel;

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

        public List<Client>? Clients { get; set; }

    }
}
