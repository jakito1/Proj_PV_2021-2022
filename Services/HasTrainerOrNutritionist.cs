using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Services
{
    public class HasTrainerOrNutritionist : IHasTrainerOrNutritionist
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public HasTrainerOrNutritionist(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<bool> ClientHasNutritionist(string? userName)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Client? client = null;
            if (user is not null)
            {
                client = await _context.Client.Include(a => a.Nutritionist).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            }
            if (client is not null && client.Nutritionist is null && !client.WantsNutritionist)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> ClientHasTrainer(string? userName)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Client? client = null;
            if (user is not null)
            {
                client = await _context.Client.Include(a => a.Trainer).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            }
            if (client is not null && client.Trainer is null && !client.WantsTrainer)
            {
                return false;
            }
            return true;
        }
    }
}
