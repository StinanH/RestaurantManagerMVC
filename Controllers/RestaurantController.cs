using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantManagerMVC.Models;
using System.Numerics;
using System.Text;

namespace RestaurantManagerMVC.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7295";

        public RestaurantController(HttpClient client)
        {
            _client = client;
        }

        //must be authorized to use this endpoint

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(int id, string sortOrder)
        {
            id = 1;

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "none";
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

        public IActionResult Create()
        {
            ViewData["Title"] = "New Restaurant";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            var json = JsonConvert.SerializeObject(restaurant);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{baseUri}/Restaurant/create", content);

            return RedirectToAction("Index");

        }
    }
}
