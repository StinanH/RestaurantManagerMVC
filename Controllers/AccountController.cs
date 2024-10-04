using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestaurantManagerMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestaurantManagerMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _client;
        private string baseUri = "https://localhost:7295";

        public AccountController(HttpClient client)
        {
            _client = client;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userLogin)
        {
            var response = await _client.PostAsJsonAsync($"{baseUri}/account/login", userLogin);

            if (!response.IsSuccessStatusCode)
            {
                return View(userLogin);
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token.Token);

            var claims = jwtToken.Claims.ToList();

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = true,
                
                ExpiresUtc = jwtToken.ValidTo
            });

            HttpContext.Response.Cookies.Append("jwtToken", token.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = jwtToken.ValidTo
            });

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwtToken");

            return RedirectToAction("Login", "Account");
        }

        ////check if there's an active cookie called jwtToken
        //public async Task<bool> IsLoggedIn()
        //{
        //    if (Request.Cookies.ContainsKey("jwtToken"))
        //    {
        //        return true;
        //        Console.WriteLine("Token found, logoutbox shown.");
        //    }

        //    else 
        //    {
        //        return false;

        //        Console.WriteLine("Token not found, loginbox shown.");
        //    }
        //}
    }
}
