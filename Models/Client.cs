using NutriFitWeb.Data;

namespace NutriFitWeb.Models
{
    public class Client
    {

        public int ClientId { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }

        public int GymId { get; set; }


        public UserAccountModel? UserAccountModel { get; set; }
    }
}
