using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using weatherSolutions.Data;
using weatherSolutions.Models;
using weatherSolutions.Controllers;
using Newtonsoft.Json;
using RestSharp;
using NuGet.Protocol;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace weatherSolutions.Controllers
{
    public class citiesController : Controller
    {
        private readonly weatherSolutionsContext _context;       

        public citiesController(weatherSolutionsContext context)
        {
            _context = context;
        }

        // GET: cities
        public async Task<IActionResult> Index()
        {
            //https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API key}
            string IDOWeather = API.OpenWeatherAPI;

            var lon = 28.1123;
            var lat = -26.2708;

            var client = new RestClient($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={IDOWeather}");
            ///RestRequest request = new RestRequest(Method.Get);
            RestRequest request = new RestRequest($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={IDOWeather}", RestSharp.Method.Get);
            RestResponse response = client.Execute(request);

            return View(await _context.city.ToListAsync());
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
                return RedirectToAction("City","cities", new { city = model.CityName });
                //return City(model.CityName);
            }
            return View(model);
        }

        public IActionResult City(string city)
        {
            string IDOWeather = API.OpenWeatherAPI;

            
            var client = new RestClient($"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&APPID={IDOWeather}");
            ///RestRequest request = new RestRequest(Method.Get);
            RestRequest request = new RestRequest($"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&APPID={IDOWeather}", RestSharp.Method.Get);
            RestResponse response = client.Execute(request);

            //WeatherResponse weatherResponse = _forecastRepository.GetForecast(city);

            if (response.IsSuccessful)
            {
                // Deserialize the response content into a CityWeatherResponse object
                //WeatherResponse weatherResponse = new WeatherResponse();
                var weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response.Content);

                // Create a view model object to pass to the view
                var viewModel = new city
                {

                    CityName = weatherResponse.Name,
                    Latitude = weatherResponse.Coord.Lat,
                    Longitude = weatherResponse.Coord.Lon,
                    Description = weatherResponse.Weather[0].Description,
                    Humidity = weatherResponse.Main.Humidity,
                    Temperature = weatherResponse.Main.Temp,
                    Temp_FeelsLike = weatherResponse.Main.Feels_Like,
                    MinTemp = weatherResponse.Main.Temp_Min,
                    MaxTemp = weatherResponse.Main.Temp_Max,
                };

                _context.Add(viewModel);

                // Save changes to persist the city entity to the database
                 _context.SaveChangesAsync();

                return View(viewModel);
               
            }
            else
            {
                return RedirectToAction("Error");   

            }

        }



        // GET: cities/Details/5
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
    }
}
