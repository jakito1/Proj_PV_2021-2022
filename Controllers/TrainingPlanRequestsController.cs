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
    public class TrainingPlanRequestsController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;

        public TrainingPlanRequestsController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
    ***REMOVED***

        [Authorize(Roles = "client, trainer")]
        public async Task<IActionResult> ShowTrainingPlanRequests(string? searchString, string? currentFilter, int? pageNumber)
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
            IQueryable<TrainingPlanRequest>? requests = null;

            if (trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanRequest.Where(a => clientIDs.Contains(a.Client.ClientId));
        ***REMOVED***

            if (client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanRequest.Where(a => a.Client == client);
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && trainer is not null && trainer.Clients is not null)
            ***REMOVED***
                clientIDs = new(trainer.Clients.Select(a => a.ClientId));
                requests = _context.TrainingPlanRequest.Where(a => clientIDs.Contains(a.Client.ClientId)).
                    Where(a => a.TrainingPlanRequestName.Contains(searchString) || a.Client.UserAccountModel.Email.Contains(searchString));
        ***REMOVED***

            if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                requests = _context.TrainingPlanRequest.Where(a => a.Client == client).Where(a => a.TrainingPlanRequestName.Contains(searchString));
        ***REMOVED***

            int pageSize = 5;
            return View(await PaginatedList<TrainingPlanRequest>.CreateAsync(requests.AsNoTracking(), pageNumber ?? 1, pageSize));
    ***REMOVED***

        // GET: TrainingPlanRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlanRequest = await _context.TrainingPlanRequest
                .FirstOrDefaultAsync(m => m.TrainingPlanRequestId == id);
            if (trainingPlanRequest == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(trainingPlanRequest);
    ***REMOVED***


        public IActionResult CreateTrainingPlanRequest(int? trainingPlanId)
        ***REMOVED***
            return View();
    ***REMOVED***

        [HttpPost, ActionName("CreateTrainingPlanRequest")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainingPlanRequestPost([Bind("TrainingPlanRequestId,TrainingPlanRequestName, TrainingPlanRequestDescription")] TrainingPlanRequest trainingPlanRequest)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Client client = await _context.Client.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
                if (client is not null)
                ***REMOVED***
                    trainingPlanRequest.Client = client;
                    trainingPlanRequest.TrainingPlanDateRequested = DateTime.Now;
                    _context.Add(trainingPlanRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowTrainingPlans", "TrainingPlans");
            ***REMOVED***                               
        ***REMOVED***
            return View(trainingPlanRequest);
    ***REMOVED***

        // GET: TrainingPlanRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlanRequest = await _context.TrainingPlanRequest.FindAsync(id);
            if (trainingPlanRequest == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(trainingPlanRequest);
    ***REMOVED***

        // POST: TrainingPlanRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingPlanRequestId,TrainingPlanRequestDescription,TrainingPlanDateRequested")] TrainingPlanRequest trainingPlanRequest)
        ***REMOVED***
            if (id != trainingPlanRequest.TrainingPlanRequestId)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            if (ModelState.IsValid)
            ***REMOVED***
                try
                ***REMOVED***
                    _context.Update(trainingPlanRequest);
                    await _context.SaveChangesAsync();
            ***REMOVED***
                catch (DbUpdateConcurrencyException)
                ***REMOVED***
                    if (!TrainingPlanRequestExists(trainingPlanRequest.TrainingPlanRequestId))
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
            return View(trainingPlanRequest);
    ***REMOVED***

        // GET: TrainingPlanRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var trainingPlanRequest = await _context.TrainingPlanRequest
                .FirstOrDefaultAsync(m => m.TrainingPlanRequestId == id);
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
            var trainingPlanRequest = await _context.TrainingPlanRequest.FindAsync(id);
            _context.TrainingPlanRequest.Remove(trainingPlanRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
    ***REMOVED***

        private bool TrainingPlanRequestExists(int id)
        ***REMOVED***
            return _context.TrainingPlanRequest.Any(e => e.TrainingPlanRequestId == id);
    ***REMOVED***
***REMOVED***
***REMOVED***
