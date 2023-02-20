using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shelters.ApplicationContext;
using Shelters.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Shelters.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataBaseContext _context;

        public HomeController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Request(string request, string title)
        {
            string connectionString = $"Server=(localdb)\\mssqllocaldb;Database=Shelters;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(request, connection);
                    var result = new RequestViewModel();
                    var reader = command.ExecuteReader();
                    result.Displays = new string[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Displays[i] = reader.GetName(i);
                    }

                    while (reader.Read())
                    {
                        string[] value = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            value[i] = reader.GetValue(i).ToString();
                        }

                        result.Result.Add(value);
                    }

                    ViewBag.RequestTitle = title;
                    return View(result);
                }
                catch (Exception)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        public IActionResult Automation()
        {
            return View(_context.Animals.Include(x => x.Shelter).Include(x => x.Owner).Where(x => x.Owner != null));
        }

        // GET: Animals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var animal = await _context.Animals.FindAsync(id);
            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Automation));
        }
    }
}