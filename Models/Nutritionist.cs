namespace NutriFitWeb.Models
{
    public class Nutritionist
    {
        public int NutritionistId { get; set; }
        public string? NutritionistFirstName { get; set; }
        public string? NutritionistLastName { get; set; }

        public Gym? Gym { get; set; }

        public UserAccountModel? UserAccountModel { get; set; }

        public List<Client>? Clients { get; set; }

    }
}
