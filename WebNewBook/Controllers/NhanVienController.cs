using Microsoft.AspNetCore.Mvc;

namespace WebNewBook.Controllers
{
    public class NhanVienController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
