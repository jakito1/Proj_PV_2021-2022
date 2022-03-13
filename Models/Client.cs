using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [DisplayName("Idade")]
        public int? ClientAge { get; set; }

        [DisplayName("Peso")]
        public double? Weight { get; set; }

        [DisplayName("Altura")]
        public double? Height { get; set; }

        [DisplayName("Ginásio")]
        public Gym? Gym { get; set; }

        public UserAccountModel? UserAccountModel { get; set; }
    }
}
