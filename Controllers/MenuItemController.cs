using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantManagerMVC.Models;
using System.Text;

namespace RestaurantManagerMVC.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7295";

        public MenuItemController(HttpClient client)
        {
            _client = client;
        }


        public IActionResult Index ()
        {
            return View();
        }


        public IActionResult Create()
        {
            ViewData["Title"] = "New menu item!";

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create (MenuItem menuItem)
        {
            var json = JsonConvert.SerializeObject(menuItem);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{baseUri}/Restaurant/1/menu/1/create", content);

            return RedirectToAction("Index", "Restaurant");

        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit (MenuItem item) 
        {
            ViewData["Title"] = "Edit menu item!";

            return View(item); ;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInDb(MenuItem menuItem)
        {
            var json = JsonConvert.SerializeObject(menuItem);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{baseUri}/Restaurant/1/menu/1/{@menuItem.Id}", content);

            if (response.IsSuccessStatusCode) {
                return RedirectToAction("Index", "Restaurant");
            }

            else
            {
                return RedirectToAction("Edit");
            }
            
        }

        [HttpPost]
        public IActionResult Delete(MenuItem item)
        {
            ViewData["Title"] = "Delete menu item?!";

            return View(item);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteInDb (int menuItemId)
        {
            if (menuItemId == null)
            {
                return RedirectToAction("Index", "Restaurant");
            }
            //var json = JsonConvert.SerializeObject(menuItem);

            //var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.DeleteAsync($"{baseUri}/Restaurant/1/menu/1/{menuItemId}");

            return RedirectToAction("Index", "Restaurant");
        }
    }
}
