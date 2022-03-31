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
            Trainer trainer = await _context.Trainer.Include(a => a.Clients).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            

            HashSet<int>? clientIDs = null;
            IQueryable<TrainingPlanNewRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanNewRequests.Where(a => clientIDs.Contains(a.Client.ClientId));
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanNewRequests.Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlanNewRequestName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString));
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanNewRequests.Where(a => a.Client == client).Where(a => a.TrainingPlanNewRequestName.Contains(searchString));
        ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlanNewRequest>.CreateAsync(requests.OrderByDescending(a => a.TrainingPlanNewRequestName).AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        // GET: TrainingPlanRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlanNewRequests = await _context.TrainingPlanNewRequests
                .FirstOrDefaultAsync(m => m.TrainingPlanNewRequestId == id);
            if (trainingPlanNewRequests == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlanNewRequests);
    ***REMOVED***


        public IActionResult CreateTrainingPlanNewRequest(int? trainingPlanId)
        ***REMOVED***
            return View();
    ***REMOVED***

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

        // GET: TrainingPlanRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlanNewRequest = await _context.TrainingPlanNewRequests.FindAsync(id);
            if (trainingPlanNewRequest == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(trainingPlanNewRequest);
    ***REMOVED***

        // POST: TrainingPlanRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingPlanRequestId,TrainingPlanRequestDescription,TrainingPlanDateRequested")] TrainingPlanNewRequest trainingPlanNewRequest)
        ***REMOVED***
            if (id != trainingPlanNewRequest.TrainingPlanNewRequestId)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            if (ModelState.IsValid)
            ***REMOVED***
                try
                ***REMOVED***
                    _context.Update(trainingPlanNewRequest);
                    await _context.SaveChangesAsync();
            ***REMOVED***
                catch (DbUpdateConcurrencyException)
                ***REMOVED***
                    if (!TrainingPlanRequestExists(trainingPlanNewRequest.TrainingPlanNewRequestId))
                    ***REMOVED***
                        return NotFound();
                ***REMOVED***
                    else
                    ***REMOVED***
                        throw;
                ***REMOVED***
            ***REMOVED***
                return RedirectToAction(nameof(Index));
        ***REMOVED***
            return View(trainingPlanNewRequest);
    ***REMOVED***

        // GET: TrainingPlanRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlanRequest = await _context.TrainingPlanNewRequests
                .FirstOrDefaultAsync(m => m.TrainingPlanNewRequestId == id);
            if (trainingPlanRequest == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlanRequest);
    ***REMOVED***

        // POST: TrainingPlanRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        ***REMOVED***
            var trainingPlanRequest = await _context.TrainingPlanNewRequests.FindAsync(id);
            _context.TrainingPlanNewRequests.Remove(trainingPlanRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
    ***REMOVED***

        private bool TrainingPlanRequestExists(int id)
        ***REMOVED***
            return _context.TrainingPlanNewRequests.Any(e => e.TrainingPlanNewRequestId == id);
    ***REMOVED***
***REMOVED***
***REMOVED***
