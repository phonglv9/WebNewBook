using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;

namespace WebNewBook.Controllers
{
    public class QLLoginController : Controller
    {
        private readonly HttpClient _httpClient;
        public QLLoginController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
        }

        public IActionResult Index(bool nhanVien)
        {
            ViewBag.nhanVien = nhanVien;
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(CustomInputModel user)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("login/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }

                error = await response.Content.ReadAsStringAsync();
                error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
            }
            ViewBag.Error = error;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserDTO user)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("login", content);

                if (response.IsSuccessStatusCode)
                {
                    string token = response.Content.ReadAsStringAsync().Result;
                    var claims = DecodedToken(token);
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    Set("token", token, 1);
                    return RedirectToAction("Index", "Home");
                }

                error = await response.Content.ReadAsStringAsync();
                error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
            }
            ViewBag.nhanVien = user.NhanVien;
            ViewBag.Error = error;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            Remove("token");
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private List<Claim> DecodedToken(string token)
        {
            var jsonToken = new JwtSecurityTokenHandler().ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS.Claims.ToList();
        }

        private void Set(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }

        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
