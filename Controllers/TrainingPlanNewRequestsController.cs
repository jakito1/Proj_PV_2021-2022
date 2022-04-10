using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    /// <summary>
    /// TrainingPlanNewRequestsController class, derives from Controller
    /// </summary>
    public class TrainingPlanNewRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public TrainingPlanNewRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Renders a paginated view to display all the new Training Plan Requests.
        /// Only accessible to Client and Trainer roles.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlanNewRequests(string? searchString, string? currentFilter, int? pageNumber)
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
            IQueryable<TrainingPlanNewRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanNewRequests.Where(a => clientIDs.Contains(a.Client.ClientId)).Where(a => a.TrainingPlanNewRequestDone == false);
            }

            if (client is not null)
            {
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client).Where(a => a.TrainingPlanNewRequestDone == false);
            }

            if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanNewRequests.Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlanNewRequestName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => a.TrainingPlanNewRequestDone == false);
            }

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client).Where(a => a.TrainingPlanNewRequestName.Contains(searchString)).
                    Where(a => a.TrainingPlanNewRequestDone == false);
            }

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlanNewRequest>.CreateAsync(requests.OrderByDescending(a => a.TrainingPlanNewRequestDate).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// Renders a view with the details of a new Training plan request.
        /// Only accessible to client and Trainer roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
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

        /// <summary>
        /// Renders a view to create a new Training Plan request.
        /// Only accessible to Client role.
        /// </summary>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client")]
        public IActionResult CreateTrainingPlanNewRequest()
        {
            return View();
        }

        /// <summary>
        /// HTTP POST action on the API to create a new Training Plan request.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="trainingPlanNewRequest"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client")]
        [HttpPost, ActionName("CreateTrainingPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanNewRequestPost([Bind("TrainingPlanNewRequestId,TrainingPlanNewRequestName, TrainingPlanNewRequestDescription")]
            TrainingPlanNewRequest trainingPlanNewRequest)
        {
            if (ModelState.IsValid)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (client is not null)
                {
                    trainingPlanNewRequest.Client = client;
                    trainingPlanNewRequest.TrainingPlanNewRequestDate = DateTime.Now;
                    _context.Add(trainingPlanNewRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowTrainingPlans", "TrainingPlans");
                }
            }
            return View(trainingPlanNewRequest);
        }

        /// <summary>
        /// Renders a view to delete a new Training Plan request.
        /// Only accesible to the Client role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client")]
        public async Task<IActionResult> DeleteTrainingPlanNewRequest(int? id)
        {
            if (id == null)
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

        /// <summary>
        /// HTTP POST action on the API to delete a new Training Plan request.
        /// Only accessible to the Client role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A RedirectToAction result</returns>
        [Authorize(Roles = "client")]
        [HttpPost, ActionName("DeleteTrainingPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanNewRequestConfirmed(int id)
        {
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
