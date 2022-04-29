using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;
using NutriFitWeb.Services;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class MachinesController : Controller
    ***REMOVED***
        private readonly string SessionKeyExercises;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IPhotoManagement _photoManagement;

        public MachinesController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IPhotoManagement photoManagement)
        ***REMOVED***
            _context = context;
            _userManager = userManager;
            SessionKeyExercises = "_Exercises";
            _photoManagement = photoManagement;
    ***REMOVED***

        [Authorize(Roles = "client, trainer, nutritionist, gym")]
        public async Task<IActionResult> ShowMachines(string? searchString, string? currentFilter, int? pageNumber)
        ***REMOVED***
            if (User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            if (searchString is not null)
            ***REMOVED***
                pageNumber = 1;
        ***REMOVED***
            else
            ***REMOVED***
                searchString = currentFilter;
        ***REMOVED***

            HttpContext.Session.Clear();

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Trainer? trainer = await _context.Trainer.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist? nutritionist = await _context.Nutritionist.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client? client = await _context.Client.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<Machine>? machines = null;

            if (!string.IsNullOrEmpty(searchString) && gym is not null)
            ***REMOVED***
                machines = _context.Machines.Where(a => a.MachineGym == gym).Where(a => a.MachineName != null && a.MachineName.Contains(searchString));
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && trainer is not null)
            ***REMOVED***
                machines = _context.Machines.Where(a => a.MachineGym == trainer.Gym).Where(a => a.MachineName != null && a.MachineName.Contains(searchString));
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && nutritionist is not null)
            ***REMOVED***
                machines = _context.Machines.Where(a => a.MachineGym == nutritionist.Gym).Where(a => a.MachineName != null && a.MachineName.Contains(searchString));
        ***REMOVED***
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            ***REMOVED***
                machines = _context.Machines.Where(a => a.MachineGym == client.Gym).Where(a => a.MachineName != null && a.MachineName.Contains(searchString));
        ***REMOVED***
            else if (gym is not null)
            ***REMOVED***
                machines = _context.Machines.Where(a => a.MachineGym == gym);
        ***REMOVED***
            else if (trainer is not null)
            ***REMOVED***
                machines = _context.Machines.Where(a => a.MachineGym == trainer.Gym);
        ***REMOVED***
            else if (nutritionist is not null)
            ***REMOVED***
                machines = _context.Machines.Where(a => a.MachineGym == nutritionist.Gym);
        ***REMOVED***
            else if (client is not null)
            ***REMOVED***
                machines = _context.Machines.Where(a => a.MachineGym == client.Gym);
        ***REMOVED***

            if (machines is not null)
            ***REMOVED***
                int pageSize = 5;
                return View(await PaginatedList<Machine>.CreateAsync(machines.AsNoTracking(), pageNumber ?? 1, pageSize));
        ***REMOVED***

            return NotFound();
    ***REMOVED***

        [AllowAnonymous]
        public async Task<IActionResult> MachineDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            List<Exercise>? exercises = await _context.Exercise.Where(a => a.Machine != null && a.Machine.MachineId == id)
                .Include(a => a.ExercisePhoto).ToListAsync();
            Machine? machine = await _context.Machines
                .Include(a => a.MachineExercises)
                .Include(a => a.MachineProfilePhoto)
                .FirstOrDefaultAsync(m => m.MachineId == id);
            if (machine == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            machine.MachineExercises = exercises;
            return View(machine);
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public IActionResult CreateMachine()
        ***REMOVED***
            return View();
    ***REMOVED***

        [Authorize(Roles = "gym")]
        [HttpPost, ActionName("CreateMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMachinePost([Bind("MachineId, MachineProfilePhoto,MachineName,MachineDescription,MachineType")] Machine machine, IFormFile? formFile)
        ***REMOVED***
            if (ModelState.IsValid && User.Identity is not null)
            ***REMOVED***
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Gym? gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);


                if (gym is not null)
                ***REMOVED***
                    List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    machine.MachineExercises = exercises;
                    machine.MachineGym = gym;
                    machine.MachineProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
                    _context.Add(machine);
                    await _context.SaveChangesAsync();
                    string baseURL = $"***REMOVED***Request.Scheme***REMOVED***://***REMOVED***Request.Host***REMOVED******REMOVED***Request.PathBase***REMOVED***";
                    machine.MachineQRCodeUri = baseURL + "/Machines/MachineDetails/" + machine.MachineId.ToString();
                    await _context.SaveChangesAsync();
            ***REMOVED***
        ***REMOVED***
            HttpContext.Session.Clear();
            return RedirectToAction("ShowMachines");
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> EditMachine(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Machine? machine = await _context.Machines
                .Include(a => a.MachineExercises)
                .Include(a => a.MachineProfilePhoto)
                .FirstOrDefaultAsync(a => a.MachineId == id);

            if (machine is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            if (machine.MachineProfilePhoto is not null)
            ***REMOVED***
                string? photoPath = _photoManagement.GetPhotoPath(machine.MachineProfilePhoto);
                machine.MachineProfilePhoto.PhotoUrl = photoPath;
        ***REMOVED***

            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, machine.MachineExercises);
            return View(machine);
    ***REMOVED***

        [Authorize(Roles = "gym")]
        [HttpPost, ActionName("EditMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMachinePost(int? id, IFormFile? formFile)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Machine? machineToUpdate = await _context.Machines.Include(a => a.MachineExercises).FirstOrDefaultAsync(a => a.MachineId == id);

            if (machineToUpdate is null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gym = await _context.Gym.Include(a => a.Machines).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (gym is not null && gym.Machines is not null && gym.Machines.Contains(machineToUpdate))
            ***REMOVED***
                Photo? oldPhoto = null;

                if (machineToUpdate.MachineProfilePhoto is not null)
                ***REMOVED***
                    oldPhoto = machineToUpdate.MachineProfilePhoto;
            ***REMOVED***
                machineToUpdate.MachineProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);

                if (await TryUpdateModelAsync<Machine>(machineToUpdate, "",
                    a => a.MachineName!, a => a.MachineDescription!, a => a.MachineType!, a => a.MachineProfilePhoto!))
                ***REMOVED***
                    List<Exercise>? exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    HttpContext.Session.Remove(SessionKeyExercises);

                    if (exercises is not null && machineToUpdate.MachineExercises is not null)
                    ***REMOVED***
                        HashSet<int>? excludedIDs = new(exercises.Select(a => a.ExerciseId));
                        IEnumerable<Exercise>? missingRows = machineToUpdate.MachineExercises.Where(a => !excludedIDs.Contains(a.ExerciseId));
                        _context.Exercise.RemoveRange(missingRows);
                ***REMOVED***

                    machineToUpdate.MachineExercises = exercises;

                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowMachines");
            ***REMOVED***

                if (machineToUpdate.MachineProfilePhoto is not null)
                ***REMOVED***
                    machineToUpdate.MachineProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
            ***REMOVED***
        ***REMOVED***
            return View(machineToUpdate);
    ***REMOVED***

        [Authorize(Roles = "gym")]
        public async Task<IActionResult> DeleteMachine(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            Machine? machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.MachineId == id);
            if (machine == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(machine);
    ***REMOVED***

        [Authorize(Roles = "gym")]
        [HttpPost, ActionName("DeleteMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMachineConfirmed(int? id)
        ***REMOVED***
            if (id is null || User.Identity is null)
            ***REMOVED***
                return BadRequest();
        ***REMOVED***

            Machine? machine = await _context.Machines.FindAsync(id);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym? gym = await _context.Gym.Include(a => a.Machines).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (gym is not null && machine is not null && gym.Machines is not null && gym.Machines.Contains(machine))
            ***REMOVED***
                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();
        ***REMOVED***

            return RedirectToAction("ShowMachines");
    ***REMOVED***
***REMOVED***
***REMOVED***
