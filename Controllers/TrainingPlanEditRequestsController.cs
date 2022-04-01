using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class TrainingPlanEditRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public TrainingPlanEditRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlanEditRequests(string? searchString, string? currentFilter, int? pageNumber)
        {
            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;

            HashSet<int>? clientIDs = null;
            IQueryable<TrainingPlanEditRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanEditRequest.Include(a => a.TrainingPlan).Where(a => clientIDs.Contains(a.Client.ClientId)).Where(a => a.TrainingPlanEditRequestDone == false);
            }

            if (client is not null)
            {
                requests = _context.TrainingPlanEditRequest.Include(a => a.TrainingPlan).Where(a => a.Client == client).Where(a => a.TrainingPlanEditRequestDone == false);
            }

            if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanEditRequest.Include(a => a.TrainingPlan).Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlan.TrainingPlanName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.TrainingPlanEditRequestDone == false);
            }

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                requests = _context.TrainingPlanEditRequest.Include(a => a.TrainingPlan).Where(a => a.Client == client).
                    Where(a => a.TrainingPlan.TrainingPlanName.Contains(searchString)).
                    Where(a => a.TrainingPlanEditRequestDone == false);
            }

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlanEditRequest>.CreateAsync(requests.OrderByDescending(a => a.TrainingPlanEditRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> TrainingPlanEditRequestDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TrainingPlanEditRequest? trainingPlanEditRequest = await _context.TrainingPlanEditRequest
                .Include(t => t.TrainingPlan)
                .FirstOrDefaultAsync(m => m.TrainingPlanEditRequestId == id);
            if (trainingPlanEditRequest == null)
            {
                return NotFound();
            }

            return View(trainingPlanEditRequest);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanEditRequest([Bind("TrainingPlanEditRequestDescription,TrainingPlanId")] TrainingPlanEditRequest trainingPlanEditRequest)
        {
            if (ModelState.IsValid)
            {
                UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client? client = await _context.Client.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                TrainingPlan? trainingPlan = await _context.TrainingPlan.FirstOrDefaultAsync(a => a.TrainingPlanId == trainingPlanEditRequest.TrainingPlanId);
                IQueryable<TrainingPlanEditRequest>? amountEditRequests = null;
                if (trainingPlan is not null)
                {
                    amountEditRequests = _context.TrainingPlanEditRequest.Where(a => a.TrainingPlan == trainingPlan).Where(a => a.TrainingPlanEditRequestDone == false);
                }

                if (client is not null && trainingPlan is not null && client.TrainingPlans.Contains(trainingPlan) &&
                    amountEditRequests is not null && !amountEditRequests.Any())
                {
                    trainingPlan.ToBeEdited = true;
                    trainingPlanEditRequest.TrainingPlanEditRequestDate = DateTime.Now;
                    trainingPlanEditRequest.Client = client;
                    _context.Add(trainingPlanEditRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("TrainingPlanDetails", "TrainingPlans", new { id = trainingPlanEditRequest.TrainingPlanId });
                }
            }
            return RedirectToAction("ShowTrainingPlans", "TrainingPlans");
        }

        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteTrainingPlanEditRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TrainingPlanEditRequest? trainingPlanEditRequest = await _context.TrainingPlanEditRequest
                .Include(t => t.TrainingPlan)
                .FirstOrDefaultAsync(m => m.TrainingPlanEditRequestId == id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanEditRequest is not null && client is not null && trainingPlanEditRequest.Client == client)
            {
                return View(trainingPlanEditRequest);
            }
            return NotFound();


        }

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteTrainingPlanEditRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanEditRequestConfirmed(int id)
        {
            TrainingPlanEditRequest? trainingPlanEditRequest = await _context.TrainingPlanEditRequest.FindAsync(id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanEditRequest is not null && client is not null && trainingPlanEditRequest.Client == client)
            {
                TrainingPlan? trainingPlan = await _context.TrainingPlan.FirstOrDefaultAsync(a => a.TrainingPlanId == trainingPlanEditRequest.TrainingPlanId);
                if (trainingPlan is not null)
                {
                    trainingPlan.ToBeEdited = false;
                }
                _context.TrainingPlanEditRequest.Remove(trainingPlanEditRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowTrainingPlanEditRequests");
            }
            return RedirectToAction("Index");
        }
    }
}
