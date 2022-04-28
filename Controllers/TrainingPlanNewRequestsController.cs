using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class TrainingPlanNewRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IInteractNotification _interactNotification;

        public TrainingPlanNewRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IInteractNotification interactNotification)
        {
            _context = context;
            _userManager = userManager;
            _interactNotification = interactNotification;
        }

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlanNewRequests(string? searchString, string? currentFilter, int? pageNumber)
        {
            if (User.Identity is null)
            {
                return BadRequest();
            }
            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainer = await _context.Trainer.Include(a => a.Clients).Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;

            HashSet<int>? clientIDs = null;
            IQueryable<TrainingPlanNewRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null && string.IsNullOrEmpty(searchString))
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId)).Where(a => a.TrainingPlanNewRequestDone == false);
            }
            else if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client != null && clientIDs.Contains(a.Client.ClientId))
                    .Where(a => a.TrainingPlanNewRequestName != null && a.TrainingPlanNewRequestName.Contains(searchString) ||
                    a.Client != null && a.Client.UserAccountModel.Email.Contains(searchString))
                    .Where(a => a.TrainingPlanNewRequestDone == false);
            }
            else if (client is not null && string.IsNullOrEmpty(searchString))
            {
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client).Where(a => a.TrainingPlanNewRequestDone == false);
            }
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client)
                    .Where(a => a.TrainingPlanNewRequestName != null && a.TrainingPlanNewRequestName.Contains(searchString))
                    .Where(a => a.TrainingPlanNewRequestDone == false);
            }

            if (requests is not null)
            {
                int pageSize = 5;
                return View(await PaginatedList<TrainingPlanNewRequest>.CreateAsync(requests.OrderByDescending(a => a.TrainingPlanNewRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            return NotFound();
        }

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> TrainingPlanNewRequestDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TrainingPlanNewRequest? trainingPlanNewRequests = await _context.TrainingPlanNewRequests
                .FirstOrDefaultAsync(m => m.TrainingPlanNewRequestId == id);
            if (trainingPlanNewRequests == null)
            {
                return NotFound();
            }

            return View(trainingPlanNewRequests);
        }

        [Authorize(Roles = "client")]
        public IActionResult CreateTrainingPlanNewRequest()
        {
            return View();
        }

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("CreateTrainingPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanNewRequestPost([Bind("TrainingPlanNewRequestId,TrainingPlanNewRequestName, TrainingPlanNewRequestDescription")]
            TrainingPlanNewRequest trainingPlanNewRequest)
        {
            if (ModelState.IsValid && User.Identity is not null)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client? client = await _context.Client.Include(a => a.Trainer!.UserAccountModel).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (client is not null && client.Trainer is not null)
                {
                    await _interactNotification.Create($"O utilizador {user.UserName} requisitou um novo plano de treino.", client.Trainer.UserAccountModel);
                    trainingPlanNewRequest.Client = client;
                    trainingPlanNewRequest.TrainingPlanNewRequestDate = DateTime.Now;
                    _context.Add(trainingPlanNewRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowTrainingPlans", "TrainingPlans");
                }
            }
            return View(trainingPlanNewRequest);
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteTrainingPlanNewRequest(int? id)
        {
            if (id is null || User.Identity is null)
            {
                return NotFound();
            }

            TrainingPlanNewRequest? trainingPlanNewRequest = await _context.TrainingPlanNewRequests
                .FirstOrDefaultAsync(m => m.TrainingPlanNewRequestId == id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanNewRequest is not null && client is not null && trainingPlanNewRequest.Client == client)
            {
                return View(trainingPlanNewRequest);
            }
            return NotFound();
        }

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteTrainingPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanNewRequestConfirmed(int? id)
        {
            if (id is null || User.Identity is null)
            {
                return BadRequest();
            }
            TrainingPlanNewRequest? trainingPlanNewRequest = await _context.TrainingPlanNewRequests.FindAsync(id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanNewRequest is not null && client is not null && trainingPlanNewRequest.Client == client)
            {
                _context.TrainingPlanNewRequests.Remove(trainingPlanNewRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowTrainingPlanNewRequests");
            }
            return RedirectToAction("Index");
        }
    }
}
