using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantManagerMVC.Models;
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
        [Authorize]
        public async Task<IActionResult> Index(string searchItem, int? searchyear)
        {
            ViewData["Title"] = "Avaliable Restaurants";

            //ser till att vi alltid tar in token
            var token = HttpContext.Request.Cookies["jwtToken"];
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //anropar API 
            var response = await _client.GetAsync($"{baseUri}/Restaurant/all_restaurants");

            //läser json som string av body
            var json = await response.Content.ReadAsStringAsync();

            //omvandlarr json till objekt List<Restaurant>();
            var restaurantList = JsonConvert.DeserializeObject<List<Restaurant>>(json);

            if (string.IsNullOrEmpty(searchItem))
            {
                int numericSearch = 0;
                int.TryParse(searchItem, out numericSearch);
            }

            return View(restaurantList);


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
