namespace NutriFitWeb.Services
***REMOVED***
    /// <summary>
    /// IHasTrainerNutritionistGym interface
    /// </summary>
    public interface IHasTrainerNutritionistGym
    ***REMOVED***
        /// <summary>
        /// Check if the Client has any Nutritionist associated and wants to have one.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>A boolen Task result</returns>
        Task<bool> ClientHasNutritionistAndWants(string? userName);
        /// <summary>
        /// Check if the Client has any Trainer associated and wants to have one.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>A boolean Task result</returns>
        Task<bool> ClientHasTrainerAndWants(string? userName);
        /// <summary>
        /// Check if the Client has any Nutritionist associated.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>A boolean Task result</returns>
        Task<bool> ClientHasNutritionist(string? userName);
        /// <summary>
        /// Check if the Client has any Trainer associated.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>A boolean Task result</returns>
        Task<bool> ClientHasTrainer(string? userName);
        /// <summary>
        /// Check if the Client has any Gym associated.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>A boolean Task result</returns>
        Task<bool> ClientHasGym(string? userName);
***REMOVED***
***REMOVED***
