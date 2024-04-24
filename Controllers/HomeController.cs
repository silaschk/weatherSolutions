using Azure.Core.GeoJson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;
using weatherSolutions.Data;
using weatherSolutions.Models;

namespace weatherSolutions.Controllers
{
    public class HomeController : Controller
    {

        private readonly weatherSolutionsContext _context;

        public HomeController(weatherSolutionsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Retrieve data from DbContext
            var cities = _context.city.ToList();

            return View(cities); // Pass data to the view

            //return View();
        }
        public IActionResult SearchCity()
        {
            var viewModel = new SearchCity();
            return View(viewModel);
        }

        // POST: ForecastApp/SearchCity
        [HttpPost]
        public IActionResult SearchCity(SearchCity model)
        {
            // If the model is valid, consume the Weather API to bring the data of the city
            if (ModelState.IsValid)
            {
                return RedirectToAction("City", "cities", new { city = model.CityName });
                //return City(model.CityName);
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.city
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CityName,Temperature,Temp_FeelsLike,MinTemp,MaxTemp,Humidity,Description,Latitude,Longitude")] city city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.city.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CityName,Temperature,Temp_FeelsLike,MinTemp,MaxTemp,Humidity,Description,Latitude,Longitude")] city city)
        {
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!cityExists(city.Id))
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
            return View(city);
        }

        // GET: cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.city
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.city.FindAsync(id);
            if (city != null)
            {
                _context.city.Remove(city);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool cityExists(int id)
        {
            return _context.city.Any(e => e.Id == id);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
