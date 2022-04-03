using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NutriFitWeb.Data;
using NutriFitWeb.Models;

namespace NutriFitWeb.Controllers
***REMOVED***
    public class MachinesController : Controller
    ***REMOVED***
        private readonly ApplicationDbContext _context;

        public MachinesController(ApplicationDbContext context)
        ***REMOVED***
            _context = context;
    ***REMOVED***

        // GET: Machines
        public async Task<IActionResult> ShowMachines()
        ***REMOVED***
            return View(await _context.Machines.ToListAsync());
    ***REMOVED***

        // GET: Machines/Details/5
        public async Task<IActionResult> MachineDetails(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.MachineId == id);
            if (machine == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(machine);
    ***REMOVED***

        // GET: Machines/Create
        public IActionResult CreateMachine()
        ***REMOVED***
            return View();
    ***REMOVED***

        // POST: Machines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("CreateMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMachinePost([Bind("MachineId,MachineName,MachineDescription,MachineType,MachineQRCodeUri")] Machine machine)
        ***REMOVED***
            if (ModelState.IsValid)
            ***REMOVED***
                _context.Add(machine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        ***REMOVED***
            return View(machine);
    ***REMOVED***

        // GET: Machines/Edit/5
        public async Task<IActionResult> EditMachine(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***
            return View(machine);
    ***REMOVED***

        // POST: Machines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("EditMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMachinePost(int id, [Bind("MachineId,MachineName,MachineDescription,MachineType,MachineQRCodeUri")] Machine machine)
        ***REMOVED***
            if (id != machine.MachineId)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            if (ModelState.IsValid)
            ***REMOVED***
                try
                ***REMOVED***
                    _context.Update(machine);
                    await _context.SaveChangesAsync();
            ***REMOVED***
                catch (DbUpdateConcurrencyException)
                ***REMOVED***
                    if (!MachineExists(machine.MachineId))
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
            return View(machine);
    ***REMOVED***

        // GET: Machines/Delete/5
        public async Task<IActionResult> DeleteMachine(int? id)
        ***REMOVED***
            if (id == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.MachineId == id);
            if (machine == null)
            ***REMOVED***
                return NotFound();
        ***REMOVED***

            return View(machine);
    ***REMOVED***

        // POST: Machines/Delete/5
        [HttpPost, ActionName("DeleteMachine")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMachineConfirmed(int id)
        ***REMOVED***
            var machine = await _context.Machines.FindAsync(id);
            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
    ***REMOVED***

        private bool MachineExists(int id)
        ***REMOVED***
            return _context.Machines.Any(e => e.MachineId == id);
    ***REMOVED***
***REMOVED***
***REMOVED***
