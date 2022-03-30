﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IIsUserInRoleByUserId _isUserInRoleByUserId;
        private readonly IPhotoManagement _photoManagement;

        public TrainersController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IIsUserInRoleByUserId inRoleByUserId,
            IPhotoManagement photoManagement)
        {
            _context = context;
            _userManager = userManager;
            _isUserInRoleByUserId = inRoleByUserId;
            _photoManagement = photoManagement;
        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ShowTrainers(string? searchString, string? currentFilter, int? pageNumber)
        {
            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);

            IOrderedQueryable<Trainer>? trainers = _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Include(a => a.Gym.UserAccountModel).
                OrderByDescending(a => a.Gym);

            if (!string.IsNullOrEmpty(searchString))
            {
                trainers = _context.Trainer.
                    Include(a => a.UserAccountModel).
                    Include(a => a.Gym).
                    Include(a => a.Gym.UserAccountModel).
                    Where(a => a.UserAccountModel.Email.Contains(searchString)).
                    OrderByDescending(a => a.Gym);
            }

            int pageSize = 3;
            return View(await PaginatedList<Trainer>.CreateAsync(trainers.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> ChangeTrainerGymStatus(int? id, int? pageNumber, string? currentFilter)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Where(a => a.UserAccountModel.Id == user.Id).FirstOrDefaultAsync();
            Trainer? trainer = await _context.Trainer.
                Include(a => a.Gym).
                Include(a => a.Clients).
                Include(a => a.TrainingPlans).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync();

            trainer.Gym = (trainer.Gym is null) ? gym : null;
            if (trainer.Gym is null)
            {
                trainer.Clients = null;
                trainer.TrainingPlans = null;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ShowTrainers", new { pageNumber, currentFilter });
        }

        public async Task<IActionResult> TrainerDetails(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            return View(await _context.Trainer.
                Include(a => a.UserAccountModel).
                Include(a => a.Gym).
                Where(a => a.TrainerId == id).
                FirstOrDefaultAsync());
        }

        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettings(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Trainer? trainer = await GetTrainer(id);
            if (trainer is not null && trainer.TrainerProfilePhoto is not null)
            {
                trainer.TrainerProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            }

            if (trainer is null)
            {
                return NotFound();
            }
            return View(trainer);
        }

        [HttpPost, ActionName("EditTrainerSettings")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "administrator, trainer")]
        public async Task<IActionResult> EditTrainerSettingsPost(string? id, IFormFile? formFile)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            Trainer? trainerToUpdate = await GetTrainer(id);

            Photo? oldPhoto = null;
            if (trainerToUpdate is not null && trainerToUpdate.TrainerProfilePhoto is not null)
            {
                oldPhoto = trainerToUpdate.TrainerProfilePhoto;
            }
            if (trainerToUpdate is not null)
            {
                trainerToUpdate.TrainerProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
            }

            if (await TryUpdateModelAsync<Trainer>(trainerToUpdate, "",
                t => t.TrainerFirstName, t => t.TrainerLastName, t => t.TrainerProfilePhoto))
            {
                if (oldPhoto is not null && trainerToUpdate.TrainerProfilePhoto is not null)
                {
                    _context.Photos.Remove(oldPhoto);
                }
                else if (trainerToUpdate.TrainerProfilePhoto is null)
                {
                    trainerToUpdate.TrainerProfilePhoto = oldPhoto;
                }

                await _context.SaveChangesAsync();
                if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
                {
                    return RedirectToAction("ShowAllUsers", "Admins");
                }
                if (trainerToUpdate.TrainerProfilePhoto is not null)
                {
                    trainerToUpdate.TrainerProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
                }
                return View(trainerToUpdate);
            }
            return View(trainerToUpdate);
        }

        private async Task<Trainer> GetTrainer(string? id)
        {
            UserAccountModel? user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (await _isUserInRoleByUserId.IsUserInRoleByUserIdAsync(user.Id, "administrator"))
            {
                return _context.Trainer.Include(a => a.TrainerProfilePhoto).FirstOrDefault(a => a.UserAccountModel.Id == id);
            }

            UserAccountModel? userAccount = await _userManager.FindByNameAsync(id);
            return await _context.Trainer.Include(a => a.TrainerProfilePhoto).FirstOrDefaultAsync(a => a.UserAccountModel == userAccount);
        }
    }
}
