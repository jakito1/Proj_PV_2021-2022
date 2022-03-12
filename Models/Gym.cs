namespace NutriFitWeb.Models
{
    public class Gym
    {
        public int GymId { get; set; }
        public string? Name { get; set; }

        public UserAccountModel? UserAccount { get; set; }

        public List<Client>? Clients { get; set; }

    }
}
