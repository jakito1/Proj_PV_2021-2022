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
    /// MachinesController class, derived from Controller
    /// </summary>
    public class MachinesController : Controller
    {
        private readonly string SessionKeyExercises;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccountModel> _userManager;
        private readonly IPhotoManagement _photoManagement;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">Application DB context</param>
        /// <param name="userManager">User manager API with Entity framework</param>
        /// <param name="photoManagement">Photo management interface</param>
        public MachinesController(ApplicationDbContext context,
            UserManager<UserAccountModel> userManager,
            IPhotoManagement photoManagement)
        {
            _context = context;
            _userManager = userManager;
            SessionKeyExercises = "_Exercises";
            _photoManagement = photoManagement;
        }

        /// <summary>
        /// Displays a page with all the machines.
        /// Only accessible to Client, Trainer, Nutritionist and Gym roles.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="currentFilter"></param>
        /// <param name="pageNumber"></param>
        /// <returns>A View result</returns>
        [Authorize(Roles = "client, trainer, nutritionist, gym")]
        public async Task<IActionResult> ShowMachines(string? searchString, string? currentFilter, int? pageNumber)
        {

            if (searchString is not null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            HttpContext.Session.Clear();
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Trainer trainer = await _context.Trainer.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Nutritionist nutritionist = await _context.Nutritionist.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);
            Client client = await _context.Client.Include(a => a.Gym).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            ViewData["CurrentFilter"] = searchString;
            IQueryable<Machine>? machines = null;

            if (!string.IsNullOrEmpty(searchString) && gym is not null)
            {
                machines = _context.Machines.Where(a => a.MachineGym == gym).Where(a => a.MachineName.Contains(searchString));
            }
            else if (!string.IsNullOrEmpty(searchString) && trainer is not null)
            {
                machines = _context.Machines.Where(a => a.MachineGym == trainer.Gym).Where(a => a.MachineName.Contains(searchString));
            }
            else if (!string.IsNullOrEmpty(searchString) && nutritionist is not null)
            {
                machines = _context.Machines.Where(a => a.MachineGym == nutritionist.Gym).Where(a => a.MachineName.Contains(searchString));
            }
            else if (!string.IsNullOrEmpty(searchString) && client is not null)
            {
                machines = _context.Machines.Where(a => a.MachineGym == client.Gym).Where(a => a.MachineName.Contains(searchString));
            }
            else if (gym is not null)
            {
                machines = _context.Machines.Where(a => a.MachineGym == gym);
            }
            else if (trainer is not null)
            {
                machines = _context.Machines.Where(a => a.MachineGym == trainer.Gym);
            }
            else if (nutritionist is not null)
            {
                machines = _context.Machines.Where(a => a.MachineGym == nutritionist.Gym);
            }
            else if (client is not null)
            {
                machines = _context.Machines.Where(a => a.MachineGym == client.Gym);
            }

            int pageSize = 5;
            return View(await PaginatedList<Machine>.CreateAsync(machines.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        /// <summary>
        /// Displays a Machine's details give the machine id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A >View result</returns>
        [AllowAnonymous]
        public async Task<IActionResult> MachineDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Machine? machine = await _context.Machines
                .Include(a => a.MachineExercises)
                .Include(a => a.MachineProfilePhoto)
                .FirstOrDefaultAsync(m => m.MachineId == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        /// <summary>
        /// Displays page to create a machine.
        /// Only accessible to the Gym role.
        /// </summary>
        /// <returns>A View result</returns>
        [Authorize(Roles = "gym")]
        public IActionResult CreateMachine()
        {
            return View();
        }

        /// <summary>
        /// HTTP POST action to create a Machine on the API.
        /// Only accessible to the Gym role.
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="formFile"></param>
        /// <returns>RedirectToAction result</returns>
        [Authorize(Roles = "gym")]
        [HttpPost, ActionName("CreateMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMachinePost([Bind("MachineId, MachineProfilePhoto,MachineName,MachineDescription,MachineType,MachineQRCodeUri")] Machine machine, IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
                Gym gym = await _context.Gym.FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);


                if (gym is not null)
                {
                    List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    machine.MachineExercises = exercises;
                    machine.MachineGym = gym;
                    machine.MachineProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
                    _context.Add(machine);
                    await _context.SaveChangesAsync();
                    string baseURL = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                    machine.MachineQRCodeUri = baseURL + "/Machines/MachineDetails/" + machine.MachineId.ToString();
                    await _context.SaveChangesAsync();
                }
            }
            HttpContext.Session.Clear();
            return RedirectToAction("ShowMachines");
        }

        /// <summary>
        /// Displays a page to edit a Machine.
        /// Only accessible to the Gym role
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "gym")]
        public async Task<IActionResult> EditMachine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Machine? machine = await _context.Machines
                .Include(a => a.MachineExercises)
                .Include(a => a.MachineProfilePhoto)
                .FirstOrDefaultAsync(a => a.MachineId == id);

            if (machine is not null && machine.MachineProfilePhoto is not null)
            {
                string? photoPath = _photoManagement.GetPhotoPath(machine.MachineProfilePhoto);
                machine.MachineProfilePhoto.PhotoUrl = photoPath;
            }

            if (machine == null)
            {
                return NotFound();
            }

            HttpContext.Session.Set<List<Exercise>>(SessionKeyExercises, machine.MachineExercises);
            return View(machine);
        }

        /// <summary>
        /// HTTP POST method to the API to edit a machine.
        /// Only accessible to the Gym role.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="formFile"></param>
        /// <returns>An action result</returns>
        [Authorize(Roles = "gym")]
        [HttpPost, ActionName("EditMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMachinePost(int? id, IFormFile? formFile)
        {
            if (id is null)
            {
                return NotFound();
            }

            Machine? machineToUpdate = await _context.Machines.Include(a => a.MachineExercises).FirstOrDefaultAsync(a => a.MachineId == id);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Include(a => a.Machines).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (gym is not null && machineToUpdate is not null && gym.Machines.Contains(machineToUpdate))
            {
                Photo? oldPhoto = null;
                if (machineToUpdate is not null && machineToUpdate.MachineProfilePhoto is not null)
                {
                    oldPhoto = machineToUpdate.MachineProfilePhoto;
                }
                if (machineToUpdate is not null)
                {
                    machineToUpdate.MachineProfilePhoto = _photoManagement.UploadProfilePhoto(formFile);
                }

                if (await TryUpdateModelAsync<Machine>(machineToUpdate, "",
                    a => a.MachineName, a => a.MachineDescription, a => a.MachineType, a => a.MachineProfilePhoto))
                {
                    List<Exercise> exercises = HttpContext.Session.Get<List<Exercise>>(SessionKeyExercises);
                    HttpContext.Session.Remove(SessionKeyExercises);

                    HashSet<int>? excludedIDs = new(exercises.Select(a => a.ExerciseId));
                    IEnumerable<Exercise>? missingRows = machineToUpdate.MachineExercises.Where(a => !excludedIDs.Contains(a.ExerciseId));

                    _context.Exercise.RemoveRange(missingRows);

                    machineToUpdate.MachineExercises = exercises;

                    await _context.SaveChangesAsync();
                    return RedirectToAction("ShowMachines");
                }

                if (machineToUpdate.MachineProfilePhoto is not null)
                {
                    machineToUpdate.MachineProfilePhoto.PhotoUrl = await _photoManagement.LoadProfileImage(User.Identity.Name);
                }
            }
            return View(machineToUpdate);
        }

        /// <summary>
        /// Displays a page to delete a machine given the id.
        /// Only accessible to the Gym roles.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "gym")]
        public async Task<IActionResult> DeleteMachine(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Machine? machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.MachineId == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        /// <summary>
        /// Action method to delete a machine and redirect to the appropriate action.
        /// Only accessible to the Gym role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An Action result</returns>
        [Authorize(Roles = "gym")]
        [HttpPost, ActionName("DeleteMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMachineConfirmed(int id)
        {
            Machine? machine = await _context.Machines.FindAsync(id);
            UserAccountModel user = await _userManager.FindByNameAsync(User.Identity.Name);
            Gym gym = await _context.Gym.Include(a => a.Machines).FirstOrDefaultAsync(a => a.UserAccountModel.Id == user.Id);

            if (gym is not null && machine is not null && gym.Machines.Contains(machine))
            {
                _context.Machines.Remove(machine);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ShowMachines");
        }
    }
}
