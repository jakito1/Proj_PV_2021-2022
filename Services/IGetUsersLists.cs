﻿using NutriFitWeb.Models;

namespace NutriFitWeb.Services
{
    public interface IGetUsersLists
    {
        IEnumerable<UserAccountModel> GetUsersForGym(string userType, string loggedIn);
        IEnumerable<UserAccountModel> GetUsersForTrainer(string loggedIn);
        string GetTrainerGym(string loggedIn);
    }
}