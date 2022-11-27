using Microsoft.AspNetCore.Mvc;

namespace WebNewBook.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }public IActionResult ForgotPassword()
        {
            return View();
        }
        
    }
}
