using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
    public class Profile_CustomerController : Controller
    {
        public async Task<IActionResult> account( KhachHang khachHang)
        {
            HttpClient _httpClient = new HttpClient(); 
            string Id_khachang = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            HttpResponseMessage response = _httpClient.GetAsync("https://localhost:7266/api/Customer/" + Id_khachang).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                khachHang = JsonConvert.DeserializeObject<KhachHang>(jsondata);
            }
            
            ViewBag.KhachHang = khachHang;
            return View();
        }

        public IActionResult profile(KhachHang khachHang)
        {
            HttpClient _httpClient = new HttpClient();
            string Id_khachang = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            HttpResponseMessage response = _httpClient.GetAsync("https://localhost:7266/api/Customer/" + Id_khachang).Result;

            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                khachHang = JsonConvert.DeserializeObject<KhachHang>(jsondata);
            }
            
            ViewBag.KhachHang = khachHang;
        
            return View();
        }
        public IActionResult order()
        {
            return View();
        }
        public IActionResult VoucherWallet()
        {
            return View();
        }
        public IActionResult FpointHistory()
        {
            // tích điểm point : 0
            // nạp point : 1
            return View();
        }

        public IActionResult OrderDetail()
        {
            return View();
        }
    }
}
