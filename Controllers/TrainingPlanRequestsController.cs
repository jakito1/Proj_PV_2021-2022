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
    public class TrainingPlanRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public TrainingPlanRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlanRequests(string? searchString, string? currentFilter, int? pageNumber)
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
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            

            HashSet<int>? clientIDs = null;
            IQueryable<TrainingPlanRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanRequest.Where(a => clientIDs.Contains(a.Client.ClientId));
            }

            if (client is not null)
            {
                requests = _context.TrainingPlanRequest.Where(a => a.Client == client);
            }

            if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            {
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanRequest.Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlanRequestName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                requests = _context.TrainingPlanRequest.Where(a => a.Client == client).Where(a => a.TrainingPlanRequestName.Contains(searchString));
            }

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlanRequest>.CreateAsync(requests.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: TrainingPlanRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlanRequest = await _context.TrainingPlanRequest
                .FirstOrDefaultAsync(m => m.TrainingPlanRequestId == id);
            if (trainingPlanRequest == null)
            {
                return NotFound();
            }

            return View(trainingPlanRequest);
        }


        public IActionResult CreateTrainingPlanRequest(int? trainingPlanId)
        {
            return View();
        }

        [HttpPost, ActionName("CreateTrainingPlanRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanRequestPost([Bind("TrainingPlanRequestId,TrainingPlanRequestName, TrainingPlanRequestDescription")] TrainingPlanRequest trainingPlanRequest)
        {
            if (ModelState.IsValid)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (client is not null)
                {
                    trainingPlanRequest.Client = client;
                    trainingPlanRequest.TrainingPlanDateRequested = DateTime.Now;
                    _context.Add(trainingPlanRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowTrainingPlans", "TrainingPlans");
                }                               
            }
            return View(trainingPlanRequest);
        }

        // GET: TrainingPlanRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlanRequest = await _context.TrainingPlanRequest.FindAsync(id);
            if (trainingPlanRequest == null)
            {
                return NotFound();
            }
            return View(trainingPlanRequest);
        }

        // POST: TrainingPlanRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingPlanRequestId,TrainingPlanRequestDescription,TrainingPlanDateRequested")] TrainingPlanRequest trainingPlanRequest)
        {
            if (id != trainingPlanRequest.TrainingPlanRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingPlanRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingPlanRequestExists(trainingPlanRequest.TrainingPlanRequestId))
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
            return View(trainingPlanRequest);
        }

        // GET: TrainingPlanRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlanRequest = await _context.TrainingPlanRequest
                .FirstOrDefaultAsync(m => m.TrainingPlanRequestId == id);
            if (trainingPlanRequest == null)
            {
                return NotFound();
            }

            return View(trainingPlanRequest);
        }

        // POST: TrainingPlanRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingPlanRequest = await _context.TrainingPlanRequest.FindAsync(id);
            _context.TrainingPlanRequest.Remove(trainingPlanRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingPlanRequestExists(int id)
        {
            return _context.TrainingPlanRequest.Any(e => e.TrainingPlanRequestId == id);
        }
    }
}
