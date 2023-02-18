using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shelters.ApplicationContext;
using Shelters.Models;

namespace Shelters.Controllers
{
    public class ShelterVolunteersController : Controller
    {
        private readonly DataBaseContext _context;

        public ShelterVolunteersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: ShelterVolunteers
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.ShelterVolunteers.Include(s => s.Shelter).Include(s => s.Volunteer);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: ShelterVolunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelterVolunteer = await _context.ShelterVolunteers
                .Include(s => s.Shelter)
                .Include(s => s.Volunteer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelterVolunteer == null)
            {
                return NotFound();
            }

            return View(shelterVolunteer);
        }

        // GET: ShelterVolunteers/Create
        public IActionResult Create()
        {
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name");
            ViewData["VolunteerId"] = new SelectList(_context.Volunteers, "Id", "Email");
            return View();
        }

        // POST: ShelterVolunteers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShelterId,VolunteerId")] ShelterVolunteer shelterVolunteer)
        {
            if (await _context.ShelterVolunteers.FirstOrDefaultAsync(x => x.ShelterId == shelterVolunteer.ShelterId
                && x.VolunteerId == shelterVolunteer.VolunteerId) is not null)
            {
                ModelState.AddModelError("VolunteerId", "Error");
            }

            if (ModelState.IsValid)
            {
                _context.Add(shelterVolunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", shelterVolunteer.ShelterId);
            ViewData["VolunteerId"] = new SelectList(_context.Volunteers, "Id", "Email", shelterVolunteer.VolunteerId);
            return View(shelterVolunteer);
        }

        // GET: ShelterVolunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelterVolunteer = await _context.ShelterVolunteers.FindAsync(id);
            if (shelterVolunteer == null)
            {
                return NotFound();
            }
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", shelterVolunteer.ShelterId);
            ViewData["VolunteerId"] = new SelectList(_context.Volunteers, "Id", "Email", shelterVolunteer.VolunteerId);
            return View(shelterVolunteer);
        }

        // POST: ShelterVolunteers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShelterId,VolunteerId")] ShelterVolunteer shelterVolunteer)
        {
            if (id != shelterVolunteer.Id)
            {
                return NotFound();
            }

            if (await _context.ShelterVolunteers.FirstOrDefaultAsync(x => x.ShelterId == shelterVolunteer.ShelterId
                && x.VolunteerId == shelterVolunteer.VolunteerId && x.Id != shelterVolunteer.Id) is not null)
            {
                ModelState.AddModelError("VolunteerId", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shelterVolunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShelterVolunteerExists(shelterVolunteer.Id))
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
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", shelterVolunteer.ShelterId);
            ViewData["VolunteerId"] = new SelectList(_context.Volunteers, "Id", "Email", shelterVolunteer.VolunteerId);
            return View(shelterVolunteer);
        }

        // GET: ShelterVolunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var shelterVolunteer = await _context.ShelterVolunteers.FindAsync(id);
            _context.ShelterVolunteers.Remove(shelterVolunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelterVolunteerExists(int id)
        {
            return _context.ShelterVolunteers.Any(e => e.Id == id);
        }
    }
}
