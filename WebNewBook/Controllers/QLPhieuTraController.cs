using Microsoft.AspNetCore.Mvc;

namespace WebNewBook.Controllers
{
    public class QLPhieuTraController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.TitleAdmin = "TRẢ HÀNG";
            return View();
        }
    }
}
