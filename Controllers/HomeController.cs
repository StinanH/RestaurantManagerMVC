using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantManagerMVC.Models;
using System.Diagnostics;

namespace RestaurantManagerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7295";

        public HomeController(ILogger<HomeController> logger, HttpClient client)
        {
            _logger = logger;
            
            _client = client;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(int id, string sortOrder)
        {
            id = 1;

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "popularity_desc";
            }

            ViewData["Title"] = "Welcomet to the Restaurant";

            //ser till att vi alltid tar in token
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //anropar API 
            var response = await _client.GetAsync($"{baseUri}/Restaurant/{id}?sortingOrder={sortOrder}");

            //läser json som string av body
            var json = await response.Content.ReadAsStringAsync();

            //omvandlarr json till objekt Restaurant;
            var restaurant = JsonConvert.DeserializeObject<Restaurant>(json);

            return View(restaurant);
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
