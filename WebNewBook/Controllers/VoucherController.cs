using Microsoft.AspNetCore.Mvc;

namespace WebNewBook.Controllers
{
    public class VoucherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
