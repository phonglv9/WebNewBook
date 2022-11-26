using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using WebNewBook.Model;
using WebNewBook.Models;
using X.PagedList;
using WebNewBook.API.ModelsAPI;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HoaDonController : Controller
    {
        Uri link = new Uri("https://localhost:7266/api");
        HttpClient client;
        public HoaDonController()
        {
            client = new HttpClient();
            client.BaseAddress = link;
            ChiTiet("HDCT1");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }
        public async Task<IActionResult> Index(string? timkiem, int? trangThai, int? page, string mess)
        {
            ViewBag.TimKiem = timkiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            var pageNumber = page ?? 1;
            List<ViewHoaDon> lissttl = new List<ViewHoaDon>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/HoaDon/GetHD").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttl = JsonConvert.DeserializeObject<List<ViewHoaDon>>(data);
            }
            if (!string.IsNullOrEmpty(timkiem))
            {
                timkiem = timkiem.ToLower();
                lissttl = lissttl.Where(a => a.hoaDon.KhachHang.HoVaTen.ToLower().Contains(timkiem)).ToList();
                
            }
            if (trangThai != null)
            {
                switch (trangThai)
                {
                    case 0:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 0).ToList();
                        break;
                    case 1:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 1).ToList();
                        break;
                    case 2:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 2).ToList();
                        break;
                    case 3:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 3).ToList();
                        break;

                    case 4:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 4).ToList();
                        break;
                    case 5:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 5).ToList();
                        break;

                    default:
                        lissttl = lissttl.ToList();
                        break;
                }
            }
            ViewBag.DataHD = lissttl.ToPagedList(pageNumber, 5);
               
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
