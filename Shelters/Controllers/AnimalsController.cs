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
    public class AnimalsController : Controller
    {
        private readonly DataBaseContext _context;

        public AnimalsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Animals
        public async Task<IActionResult> Index(string name, string species, string gender, int ageFrom, int ageTo,
            string shelter, string owner, AnimalSort sort = AnimalSort.NameAsc)
        {
            IQueryable<Animal> animals = _context.Animals.Include(a => a.Shelter).Include(x => x.Owner);

            if (!String.IsNullOrEmpty(name))
            {
                animals = animals.Where(x => x.Name.Contains(name));
            }

            if (!String.IsNullOrEmpty(species))
            {
                animals = animals.Where(x => x.Species.Contains(species));
            }

            if (!String.IsNullOrEmpty(gender))
            {
                animals = animals.Where(x => x.Gender.Contains(gender));
            }

            animals = animals.Where(x => x.Age >= ageFrom);
            if (ageTo != 0)
            {
                animals = animals.Where(x => x.Age <= ageTo);
            }

            if (!String.IsNullOrEmpty(shelter))
            {
                animals = animals.Where(x => x.Shelter.Name.Contains(shelter));
            }

            switch (owner)
            {
                case "1":
                    animals = animals.Where(x => x.OwnerId != null);
                    break;
                case "0":
                    animals = animals.Where(x => x.OwnerId == null);
                    break;
            }

            switch (sort)
            {
                case AnimalSort.NameDesc:
                    animals = animals.OrderByDescending(x => x.Name);
                    break;
                case AnimalSort.SpeciesAsc:
                    animals = animals.OrderBy(x => x.Species);
                    break;
                case AnimalSort.SpeciesDesc:
                    animals = animals.OrderByDescending(x => x.Species);
                    break;
                case AnimalSort.GenderAsc:
                    animals = animals.OrderBy(x => x.Gender);
                    break;
                case AnimalSort.GenderDesc:
                    animals = animals.OrderByDescending(x => x.Gender);
                    break;
                case AnimalSort.AgeAsc:
                    animals = animals.OrderBy(x => x.Age);
                    break;
                case AnimalSort.AgeDesc:
                    animals = animals.OrderByDescending(x => x.Age);
                    break;
                case AnimalSort.ShelterNameAsc:
                    animals = animals.OrderBy(x => x.Shelter.Name);
                    break;
                case AnimalSort.ShelterNameDesc:
                    animals = animals.OrderByDescending(x => x.Shelter.Name);
                    break;
                default:
                    animals = animals.OrderBy(x => x.Name);
                    break;
            }

            List<SelectListItem> owners = new List<SelectListItem>();
            owners.Add(new SelectListItem("Select owner", "-1"));
            owners.Add(new SelectListItem("Has owner", "1"));
            owners.Add(new SelectListItem("No owner", "0"));

            ViewBag.Owner = new SelectList(owners, "Value", "Text", owner);
            ViewBag.Name = name;
            ViewBag.Species = species;
            ViewBag.Gender = gender;
            ViewBag.AgeFrom = ageFrom;
            ViewBag.AgeTo = ageTo;
            ViewBag.Shelter = shelter;
            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(AnimalSort)).Cast<AnimalSort>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

            return View(await animals.ToListAsync());
        }

        // GET: Animals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.Shelter)
                .Include(x => x.Owner)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animals/Create
        public IActionResult Create()
        {
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name");
            List<SelectListItem> owners = new List<SelectListItem>();
            owners.Add(new SelectListItem("No owner", null));
            foreach(Owner owner in _context.Owners.ToList())
            {
                owners.Add(new SelectListItem(owner.Email, owner.Id.ToString()));
            }

            ViewData["OwnerId"] = new SelectList(owners, "Value", "Text");
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Species,Gender,Age,Description,ShelterId,OwnerId")] Animal animal)
        {
            if (await _context.Animals.FirstOrDefaultAsync(x => x.Name == animal.Name
                && x.ShelterId != animal.ShelterId) is not null)
            {
                ModelState.AddModelError("Name", "The name is already in the database ");
            }

            if (ModelState.IsValid)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", animal.ShelterId);
            List<SelectListItem> owners = new List<SelectListItem>();
            owners.Add(new SelectListItem("No owner", null));
            foreach (Owner owner in _context.Owners.ToList())
            {
                owners.Add(new SelectListItem(owner.Email, owner.Id.ToString()));
            }

            ViewData["OwnerId"] = new SelectList(owners, "Value", "Text", animal.OwnerId);
            return View(animal);
        }

        // GET: Animals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", animal.ShelterId);
            List<SelectListItem> owners = new List<SelectListItem>();
            owners.Add(new SelectListItem("No owner", null));
            foreach (Owner owner in _context.Owners.ToList())
            {
                owners.Add(new SelectListItem(owner.Email, owner.Id.ToString()));
            }

            ViewData["OwnerId"] = new SelectList(owners, "Value", "Text", animal.OwnerId);
            return View(animal);
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Species,Gender,Age,Description,ShelterId,OwnerId")] Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (await _context.Animals.FirstOrDefaultAsync(x => x.Name == animal.Name
                && x.ShelterId != animal.ShelterId && x.Id != animal.Id) is not null)
            {
                ModelState.AddModelError("Name", "The name is already in the database ");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
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
            ViewData["ShelterId"] = new SelectList(_context.Shelters, "Id", "Name", animal.ShelterId);
            List<SelectListItem> owners = new List<SelectListItem>();
            owners.Add(new SelectListItem("No owner", null));
            foreach (Owner owner in _context.Owners.ToList())
            {
                owners.Add(new SelectListItem(owner.Email, owner.Id.ToString()));
            }

            ViewData["OwnerId"] = new SelectList(owners, "Value", "Text", animal.OwnerId);
            return View(animal);
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var animal = await _context.Animals.FindAsync(id);
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}
