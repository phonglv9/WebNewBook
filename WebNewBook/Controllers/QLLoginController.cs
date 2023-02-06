using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel user)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("login/ChangePassword", content);

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

        [Authorize]
        public IActionResult ChangePasswordNV()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordNV(ChangePasswordModel user)
        {
            string error = "";
            user.NhanVien = true;
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("login/ChangePassword", content);

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
        public async Task<IActionResult> ForgotPassword(string user)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsync("login/ForgotPassword?tk=" + user, null);

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
        public async Task<IActionResult> Login(UserDTO user)
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
           
            ViewBag.Error = error;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            Remove("token");
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove("idVoucher");
            HttpContext.Session.Remove("amoutVoucher");
           
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
