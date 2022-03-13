namespace NutriFitWeb.Models
{
    public class Trainer
    {
        public int TrainerId { get; set; }
        public string? TrainerFirstName { get; set; }
        public string? TrainerLastName { get; set; }

        public Gym? Gym { get; set; }

        public UserAccountModel? UserAccountModel { get; set; }

        public List<Client>? Clients { get; set; }

    }
}
