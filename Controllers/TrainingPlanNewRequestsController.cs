using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class TrainingPlanNewRequestsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public TrainingPlanNewRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
    ***REMOVED***

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlanNewRequests(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***
            if (searchString is not null)
            ***REMOVED***
                pageNumber = 1;
        ***REMOVED***
            else
            ***REMOVED***
                searchString = currentFilter;
        ***REMOVED***

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.Include(a => a.TrainingPlans).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;


            HashSet<int>? clientIDs = null;
            IQueryable<TrainingPlanNewRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanNewRequests.Where(a => clientIDs.Contains(a.Client.ClientId)).Where(a => a.TrainingPlanNewRequestDone == false);
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client).Where(a => a.TrainingPlanNewRequestDone == false);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanNewRequests.Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlanNewRequestName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.TrainingPlanNewRequestDone == false);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client).Where(a => a.TrainingPlanNewRequestName.Contains(searchString)).
                    Where(a => a.TrainingPlanNewRequestDone == false);
        ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlanNewRequest>.CreateAsync(requests.OrderByDescending(a => a.TrainingPlanNewRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> TrainingPlanNewRequestDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlanNewRequest? trainingPlanNewRequests = await _context.TrainingPlanNewRequests
                .FirstOrDefaultAsync(m => m.TrainingPlanNewRequestId == id);
            if (trainingPlanNewRequests == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlanNewRequests);
    ***REMOVED***

        [Authorize(Roles = "client")]
        public IActionResult CreateTrainingPlanNewRequest()
        ***REMOVED***
            return View();
    ***REMOVED***

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("CreateTrainingPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanNewRequestPost([Bind("TrainingPlanNewRequestId,TrainingPlanNewRequestName, TrainingPlanNewRequestDescription")]
            TrainingPlanNewRequest trainingPlanNewRequest)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (client is not null)
                ***REMOVED***
                    trainingPlanNewRequest.Client = client;
                    trainingPlanNewRequest.TrainingPlanNewRequestDate = DateTime.Now;
                    _context.Add(trainingPlanNewRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowTrainingPlans", "TrainingPlans");
            ***REMOVED***
        ***REMOVED***
            return View(trainingPlanNewRequest);
    ***REMOVED***

        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteTrainingPlanNewRequest(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            TrainingPlanNewRequest? trainingPlanNewRequest = await _context.TrainingPlanNewRequests
                .FirstOrDefaultAsync(m => m.TrainingPlanNewRequestId == id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanNewRequest is not null && client is not null && trainingPlanNewRequest.Client == client)
            ***REMOVED***
                return View(trainingPlanNewRequest);
        ***REMOVED***
            return NotFound();
    ***REMOVED***

        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteTrainingPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanNewRequestConfirmed(int id)
        ***REMOVED***
            TrainingPlanNewRequest? trainingPlanNewRequest = await _context.TrainingPlanNewRequests.FindAsync(id);

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Client? client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel == user);

            if (trainingPlanNewRequest is not null && client is not null && trainingPlanNewRequest.Client == client)
            ***REMOVED***
                _context.TrainingPlanNewRequests.Remove(trainingPlanNewRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("ShowTrainingPlanNewRequests");
        ***REMOVED***
            return RedirectToAction("Index");
    ***REMOVED***
***REMOVED***
***REMOVED***
