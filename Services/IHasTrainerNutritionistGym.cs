namespace NutriFitWeb.Services
{
    public interface IHasTrainerNutritionistGym
    {
        Task<bool> ClientHasNutritionistAndWants(string? userName);
        Task<bool> ClientHasTrainerAndWants(string? userName);
        Task<bool> ClientHasNutritionist(string? userName);
        Task<bool> ClientHasTrainer(string? userName);
        Task<bool> ClientHasGym(string? userName);
    }
}
