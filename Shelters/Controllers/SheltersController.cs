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
    public class SheltersController : Controller
    {
        private readonly DataBaseContext _context;

        public SheltersController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Shelters
        public async Task<IActionResult> Index(string name, string address, string city, string phone, string site,
            int countFrom, int countTo, ShelterSort sort = ShelterSort.NameAsc)
        {
            IQueryable<Shelter> shelters = _context.Shelters.Include(x => x.Animals);

            if (!String.IsNullOrEmpty(name))
            {
                shelters = shelters.Where(x => x.Name.Contains(name));
            }

            if (!String.IsNullOrEmpty(address))
            {
                shelters = shelters.Where(x => x.Address.Contains(address));
            }

            if (!String.IsNullOrEmpty(city))
            {
                shelters = shelters.Where(x => x.City.Contains(city));
            }

            if (!String.IsNullOrEmpty(phone))
            {
                shelters = shelters.Where(x => x.Phone.Contains(phone));
            }

            if (!String.IsNullOrEmpty(site))
            {
                shelters = shelters.Where(x => x.Website.Contains(site));
            }

            shelters = shelters.Where(x => x.Animals.Count() >= countFrom);
            if (countTo != 0)
            {
                shelters = shelters.Where(x => x.Animals.Count() <= countTo);
            }

            switch (sort)
            {
                case ShelterSort.NameDesc:
                    shelters = shelters.OrderByDescending(x => x.Name);
                    break;
                case ShelterSort.AddressAsc:
                    shelters = shelters.OrderBy(x => x.Address);
                    break;
                case ShelterSort.AddressDesc:
                    shelters = shelters.OrderByDescending(x => x.Address);
                    break;
                case ShelterSort.CityAsc:
                    shelters = shelters.OrderBy(x => x.City);
                    break;
                case ShelterSort.CityDesc:
                    shelters = shelters.OrderByDescending(x => x.City);
                    break;
                case ShelterSort.PhoneAsc:
                    shelters = shelters.OrderBy(x => x.Phone);
                    break;
                case ShelterSort.PhoneDesc:
                    shelters = shelters.OrderByDescending(x => x.Phone);
                    break;
                case ShelterSort.WebsiteAsc:
                    shelters = shelters.OrderBy(x => x.Website);
                    break;
                case ShelterSort.WebsiteDesc:
                    shelters = shelters.OrderByDescending(x => x.Website);
                    break;
                default:
                    shelters = shelters.OrderBy(x => x.Name);
                    break;
            }

            ViewBag.Name = name;
            ViewBag.Address = address;
            ViewBag.City = city;
            ViewBag.Phone = phone;
            ViewBag.Site = site;
            ViewBag.CountFrom = countFrom;
            ViewBag.CountTo = countTo;
            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(ShelterSort)).Cast<ShelterSort>()
               .Select(x => new SelectListItem
               {
                   Text = x.ToString(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

            return View(await shelters.ToListAsync());
        }

        // GET: Shelters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _context.Shelters
                .Include(x => x.ShelterVolunteers)
                .ThenInclude(x => x.Volunteer)
                .Include(x => x.WishLists)
                .Include(x => x.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shelter == null)
            {
                return NotFound();
            }

            return View(shelter);
        }

        // GET: Shelters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shelters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,City,Phone,Website")] Shelter shelter)
        {
            if (await _context.Shelters.FirstOrDefaultAsync(x => x.Name == shelter.Name) is not null)
            {
                ModelState.AddModelError("Email", "The name is already in the database");
            }

            if (ModelState.IsValid)
            {
                _context.Add(shelter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shelter);
        }

        // GET: Shelters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shelter = await _context.Shelters.FindAsync(id);
            if (shelter == null)
            {
                return NotFound();
            }
            return View(shelter);
        }

        // POST: Shelters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,City,Phone,Website")] Shelter shelter)
        {
            if (id != shelter.Id)
            {
                return NotFound();
            }

            if (await _context.Shelters.FirstOrDefaultAsync(x => x.Name == shelter.Name && x.Id != shelter.Id) is not null)
            {
                ModelState.AddModelError("Email", "The name is already in the database");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shelter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShelterExists(shelter.Id))
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
            return View(shelter);
        }

        // GET: Shelters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var shelter = await _context.Shelters.FindAsync(id);
            _context.Shelters.Remove(shelter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShelterExists(int id)
        {
            return _context.Shelters.Any(e => e.Id == id);
        }
    }
}
