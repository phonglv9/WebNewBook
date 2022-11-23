using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebNewBook.Model;
using WebNewBook.Models;
using X.PagedList;

namespace WebNewBook.Controllers
{
    public class HoaDonController : Controller
    {
        Uri link = new Uri("https://localhost:7266/api");
        HttpClient client;
        public HoaDonController()
        {
            client = new HttpClient();
            client.BaseAddress = link;
        }
        public async Task<IActionResult> Index(string? timkiem)
        {
           
            List<ViewHoaDon> lissttl = new List<ViewHoaDon>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/HoaDon/GetHD").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttl = JsonConvert.DeserializeObject<List<ViewHoaDon>>(data);
            }
            if (!string.IsNullOrEmpty(timkiem))
            {
                lissttl = lissttl.Where(a => a.hoaDon.KhachHang.HoVaTen.Contains(timkiem)).ToList();
                
            }
            ViewBag.data = lissttl;
            //ViewBag.dataNew = lissttl.ToPagedList((int)page, (int)pagesize);
            return View("IndexHD");

        }
        public async Task<IActionResult> ChiTiet(string id)
        {
            ViewHoaDonCT lissttl = new ViewHoaDonCT();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/HoaDon/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttl = JsonConvert.DeserializeObject<ViewHoaDonCT>(data);
            }


            ViewBag.datahdct = lissttl;

            return View("IndexHDCT", lissttl);
        }
        public async Task<IActionResult> Sua(string id, int name)
        {
            HoaDon lissttl = new HoaDon();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/HoaDon/UpdateTT/{id}/{name}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttl = JsonConvert.DeserializeObject<HoaDon>(data);
            }

            
            ViewBag.datahdct = lissttl;

            return RedirectToAction(nameof(Index));
        }
    }
}
