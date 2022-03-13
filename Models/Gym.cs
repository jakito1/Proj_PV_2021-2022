namespace NutriFitWeb.Models
{
    public class Gym
    {
        public int GymId { get; set; }
        public string? GymName { get; set; }

        public UserAccountModel? UserAccount { get; set; }

        public List<Client>? Clients { get; set; }
        public List<Nutritionist>? Nutritionists { get; set; }
        public List<Trainer>? Trainers { get; set; }

    }
}
