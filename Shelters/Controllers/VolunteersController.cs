using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shelters.ApplicationContext;
using Shelters.Enums;
using Shelters.Models;

namespace Shelters.Controllers
{
    public class VolunteersController : Controller
    {
        private readonly DataBaseContext _context;

        public VolunteersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Volunteers
        public async Task<IActionResult> Index(string name, string email, string phone, DateTime dateFrom, DateTime dateTo,
            string shelter, VolunteerSort sort = VolunteerSort.NameAsc)
        {
            IQueryable<Volunteer> vols = _context.Volunteers.Include(x => x.ShelterVolunteers).ThenInclude(x => x.Shelter);
            if (!String.IsNullOrEmpty(name))
            {
                vols = vols.Where(x => x.Name.Contains(name));
            }

            if (!String.IsNullOrEmpty(email))
            {
                vols = vols.Where(x => x.Email.Contains(email));
            }

            if (!String.IsNullOrEmpty(phone))
            {
                vols = vols.Where(x => x.Phone.Contains(phone));
            }

            vols = vols.Where(x => x.Birthdate >= dateFrom);
            if (dateTo.Year != 1)
            {
                vols = vols.Where(x => x.Birthdate <= dateTo);
            }

            if (!String.IsNullOrEmpty(shelter))
            {
                vols = vols.Where(x => x.ShelterVolunteers.FirstOrDefault(c => c.Shelter.Name.Contains(shelter)) != null);
            }

            switch (sort)
            {
                case VolunteerSort.NameDesc:
                    vols = vols.OrderByDescending(x => x.Name);
                    break;
                case VolunteerSort.EmailAsc:
                    vols = vols.OrderBy(x => x.Email);
                    break;
                case VolunteerSort.EmailDesc:
                    vols = vols.OrderByDescending(x => x.Email);
                    break;
                case VolunteerSort.PhoneAsc:
                    vols = vols.OrderBy(x => x.Phone);
                    break;
                case VolunteerSort.PhoneDesc:
                    vols = vols.OrderByDescending(x => x.Phone);
                    break;
                case VolunteerSort.BirthdayAsc:
                    vols = vols.OrderBy(x => x.Birthdate);
                    break;
                case VolunteerSort.BirthdayDesc:
                    vols = vols.OrderByDescending(x => x.Birthdate);
                    break;
                case VolunteerSort.SkillsAsc:
                    vols = vols.OrderBy(x => x.Skills);
                    break;
                case VolunteerSort.SkillsDesc:
                    vols = vols.OrderByDescending(x => x.Skills);
                    break;
                default:
                    vols = vols.OrderBy(x => x.Name);
                    break;
            }

            ViewBag.Name = name;
            ViewBag.Email = email;
            ViewBag.Phone = phone;
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;
            ViewBag.Shelter = shelter;
            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(VolunteerSort)).Cast<VolunteerSort>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

            return View(await vols.ToListAsync());
        }

        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .Include(x => x.ShelterVolunteers)
                .ThenInclude(x => x.Shelter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // GET: Volunteers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Phone,Birthdate,Skills")] Volunteer volunteer)
        {
            if (await _context.Volunteers.FirstOrDefaultAsync(x => x.Email == volunteer.Email) is not null)
            {
                ModelState.AddModelError("Email", "The email is already in the database");
            }

            if (ModelState.IsValid)
            {
                _context.Add(volunteer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Phone,Birthdate,Skills")] Volunteer volunteer)
        {
            if (id != volunteer.Id)
            {
                return NotFound();
            }

            if (await _context.Volunteers.FirstOrDefaultAsync(x => x.Email == volunteer.Email && x.Id != volunteer.Id) is not null)
            {
                ModelState.AddModelError("Email", "The email is already in the database");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.Id))
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
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            _context.Volunteers.Remove(volunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerExists(int id)
        {
            return _context.Volunteers.Any(e => e.Id == id);
        }
    }
}
