using NutriFitWeb.Data;

namespace NutriFitWeb.Models
{
    public class Client
    {

        public int ClientId { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }

        public Gym? Gym { get; set; }


        public UserAccountModel? UserAccountModel { get; set; }
    }
}
