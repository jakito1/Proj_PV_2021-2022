namespace NutriFitWeb.Services
{
    public interface IHasTrainerOrNutritionist
    {
        Task<bool> ClientHasNutritionist(string? userName);
        Task<bool> ClientHasTrainer(string? userName);
    }
}
