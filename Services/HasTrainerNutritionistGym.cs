using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Services
***REMOVED***
    public class HasTrainerNutritionistGym : IHasTrainerNutritionistGym
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        public HasTrainerNutritionistGym(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
    ***REMOVED***

        public async Task<bool> ClientHasNutritionistAndWants(string? userName)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Client? client = null;
            if (user is not null)
            ***REMOVED***
                client = await _context.Client.Include(a => a.Nutritionist).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
        ***REMOVED***
            if (client is not null && client.Nutritionist is null && !client.WantsNutritionist)
            ***REMOVED***
                return true;
        ***REMOVED***
            return false;
    ***REMOVED***

        public async Task<bool> ClientHasTrainerAndWants(string? userName)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Client? client = null;
            if (user is not null)
            ***REMOVED***
                client = await _context.Client.Include(a => a.Trainer).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
        ***REMOVED***
            if (client is not null && client.Trainer is null && !client.WantsTrainer)
            ***REMOVED***
                return true;
        ***REMOVED***
            return false;
    ***REMOVED***

        public async Task<bool> ClientHasGym(string? userName)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Client? client = null;
            if (user is not null)
            ***REMOVED***
                client = await _context.Client.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
        ***REMOVED***
            if (client is not null && client.Gym is null)
            ***REMOVED***
                return false;
        ***REMOVED***
            return true;
    ***REMOVED***

        public async Task<bool> ClientHasNutritionist(string? userName)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Client? client = null;
            if (user is not null)
            ***REMOVED***
                client = await _context.Client.Include(a => a.Nutritionist).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
        ***REMOVED***
            if (client is not null && client.Nutritionist is not null)
            ***REMOVED***
                return true;
        ***REMOVED***
            return false;
    ***REMOVED***

        public async Task<bool> ClientHasTrainer(string? userName)
        ***REMOVED***
            UserAccountModel? user = await _userManager.FindByNameAsync(userName);
            Client? client = null;
            if (user is not null)
            ***REMOVED***
                client = await _context.Client.Include(a => a.Trainer).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
        ***REMOVED***
            if (client is not null && client.Trainer is not null)
            ***REMOVED***
                return true;
        ***REMOVED***
            return false;
    ***REMOVED***
***REMOVED***
***REMOVED***
