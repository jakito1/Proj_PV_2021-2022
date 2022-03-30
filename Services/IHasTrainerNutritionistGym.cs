namespace NutriFitWeb.Services
{
    public interface IHasTrainerNutritionistGym
    {
        Task<bool> ClientHasNutritionist(string? userName);
        Task<bool> ClientHasTrainer(string? userName);
        Task<bool> ClientHasGym(string? userName);
    }
}
