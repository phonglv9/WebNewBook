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
using Org.BouncyCastle.Asn1.Ocsp;

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
            //ChiTiet("HDCT1");
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
                lissttl = lissttl.Where(a => a.hoaDon.ID_HoaDon.ToLower().Contains(timkiem)).ToList();
                
            }

            //var checkbox = Request.Form["CheckBoxId"];
            //if (checkbox.Contains("true"))
            //{
            //    lissttl = lissttl.Where(a => a.KhachHang.ID_KhachHang.ToLower().Contains("KHNOLOGIN")).ToList();

            //}
            if (trangThai != null)
            {
                switch (trangThai)
                {
                    case 6:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 6).ToList();
                        break;
                    //case 1:
                    //    lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 1).ToList();
                    //    break;
                    //case 2:
                    //    lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 2).ToList();
                    //    break;
                    //case 3:
                    //    lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 3).ToList();
                    //    break;

                    //case 4:
                    //    lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 4).ToList();
                    //    break;
                    case 5:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 5).ToList();
                        break;
                    case 7:
                        lissttl = lissttl.Where(a => a.KhachHang.ID_KhachHang.ToLower().Contains("KHNOLOGIN")).ToList();
                        break;

                    default:
                        lissttl = lissttl.ToList();
                        break;
                }
            }
            List<ViewHoaDon> lissttl2 = new List<ViewHoaDon>();
            foreach(var item in lissttl)
            {
                if (item.hoaDon.TrangThai != 3)
                {
                    lissttl2.Add(item);
                }
            }

            ViewBag.DataHD = lissttl2.ToPagedList(pageNumber, 5);
               
                //ViewBag.dataNew = lissttl.ToPagedList((int)page, (int)pagesize);
                return View("IndexHD");
            

        }
        public async Task<IActionResult> ChiTiet(string id)
        {
            List<ViewHoaDonCT> lissttlhdct = new List<ViewHoaDonCT>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/HoaDon/getlistid/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttlhdct = JsonConvert.DeserializeObject<List<ViewHoaDonCT>>(data);
                ViewBag.Namekh = lissttlhdct.Where(c=>c.hoaDon.ID_HoaDon==id).Select(c=>c.KhachHang.HoVaTen).FirstOrDefault();
                ViewBag.sdtkh = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.SDT).FirstOrDefault();
                ViewBag.ghichu = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.GhiChu).FirstOrDefault();
                ViewBag.diachi = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.DiaChiGiaoHang).FirstOrDefault();
                ViewBag.ngaymua = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.NgayMua).FirstOrDefault();
                ViewBag.tongtien= lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.TongTien).FirstOrDefault() ;
                

            }

            ViewHoaDonCT a = new ViewHoaDonCT();
           

            return View("IndexHDCT", lissttlhdct);
        }
        public async Task<IActionResult> Sua(string id, int name)
        {
            
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/HoaDon/UpdateTT/{id}/{name}").Result;
           

            return RedirectToAction("Index");
        }
    }
}
