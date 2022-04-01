using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public TrainingPlanNewRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

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
            HashSet<int?>? trainingPlanIDs = null;
            IQueryable<TrainingPlanNewRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                trainingPlanIDs = new(trainer.TrainingPlans.Select(a => a.TrainingPlanNewRequestId));
                requests = _context.TrainingPlanNewRequests.Where(a => clientIDs.Contains(a.Client.ClientId)).Where(a => !trainingPlanIDs.Contains(a.TrainingPlanNewRequestId));
            }

            if (client is not null)
            {
                trainingPlanIDs = new(client.TrainingPlans.Select(a => a.TrainingPlanNewRequestId));
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client).Where(a => !trainingPlanIDs.Contains(a.TrainingPlanNewRequestId));
            }

            if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                trainingPlanIDs = new(trainer.TrainingPlans.Select(a => a.TrainingPlanNewRequestId));
                requests = _context.TrainingPlanNewRequests.Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlanNewRequestName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString)).
                    Where(a => !trainingPlanIDs.Contains(a.TrainingPlanNewRequestId));
            }

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                trainingPlanIDs = new(client.TrainingPlans.Select(a => a.TrainingPlanNewRequestId));
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client).Where(a => a.TrainingPlanNewRequestName.Contains(searchString)).
                    Where(a => !trainingPlanIDs.Contains(a.TrainingPlanNewRequestId));
            }

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlanNewRequest>.CreateAsync(requests.OrderByDescending(a => a.TrainingPlanNewRequestName).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> TrainingPlanNewRequestDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlanNewRequests = await _context.TrainingPlanNewRequests
                .FirstOrDefaultAsync(m => m.TrainingPlanNewRequestId == id);
            if (trainingPlanNewRequests == null)
            {
                return NotFound();
            }

            return View(trainingPlanNewRequests);
        }


        public IActionResult CreateTrainingPlanNewRequest(int? trainingPlanId)
        {
            return View();
        }

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

        public async Task<IActionResult> EditTrainingPlanNewRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlanNewRequest = await _context.TrainingPlanNewRequests.FindAsync(id);
            if (trainingPlanNewRequest == null)
            {
                return NotFound();
            }
            return View(trainingPlanNewRequest);
        }

        [HttpPost, ActionName("TrainingPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrainingPlanNewRequestPost(int id, [Bind("TrainingPlanRequestId,TrainingPlanRequestDescription,TrainingPlanDateRequested")] TrainingPlanNewRequest trainingPlanNewRequest)
        {
            if (id != trainingPlanNewRequest.TrainingPlanNewRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingPlanNewRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingPlanRequestExists(trainingPlanNewRequest.TrainingPlanNewRequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trainingPlanNewRequest);
        }

        public async Task<IActionResult> DeleteTrainingPlanNewRequest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlanRequest = await _context.TrainingPlanNewRequests
                .FirstOrDefaultAsync(m => m.TrainingPlanNewRequestId == id);
            if (trainingPlanRequest == null)
            {
                return NotFound();
            }

            return View(trainingPlanRequest);
        }

        [HttpPost, ActionName("DeleteTrainingPlanNewRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingPlanNewRequestConfirmed(int id)
        {
            var trainingPlanRequest = await _context.TrainingPlanNewRequests.FindAsync(id);
            _context.TrainingPlanNewRequests.Remove(trainingPlanRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingPlanRequestExists(int id)
        {
            return _context.TrainingPlanNewRequests.Any(e => e.TrainingPlanNewRequestId == id);
        }
    }
}
