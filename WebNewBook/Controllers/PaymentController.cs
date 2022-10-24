using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using WebNewBook.Models;

namespace WebNewBook.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public List<CartItem> Giohangs
        {
            get
            {
                List<CartItem> data = new List<CartItem>();
                var jsonData = Request.Cookies["Cart"];
                if (jsonData != null)
                {
                    data = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
                }             
                return data;
            }
        }
        public IActionResult CheckOut() {

            ViewBag.Cart =  Giohangs;
            ViewBag.TongTien = Giohangs.Sum(c => c.ThanhTien);


          
            return View();
        }
    }
}
