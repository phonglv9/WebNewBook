using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Models;

namespace WebNewBook.Controllers
{

    public class GioHangController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;

        public GioHangController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;

            ListData();

        }


        public async Task<IActionResult> GetSanPham()
        {
            //Model home
            List<SanPham> modelHome = new List<SanPham>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/giohang/SanPham").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelHome = JsonConvert.DeserializeObject<List<SanPham>>(jsonData);


            };



            return View(modelHome);
        }
        public List<HomeVM> ListData()
        {
            List<HomeVM> modelHome = new List<HomeVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/GioHang/GioHangVM").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelHome = JsonConvert.DeserializeObject<List<HomeVM>>(jsonData);


            };
            return modelHome;

        }

        public List<CartItem> Giohangs
        {
            get
            {
                List<CartItem> data = new List<CartItem>();
                var opt = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
                var jsonData = Request.Cookies["Cart"];
                if (jsonData != null)
                {
                    data = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
                }









                if (data == null)
                {
                    return data;



                }

                return data;
            }
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.giohang = Giohangs;
            ViewBag.thanhtien = Giohangs.Sum(a => a.ThanhTien);
            ViewBag.soluong = Giohangs.Sum(a => a.Soluong);

            return View("Index");
        }




        public IActionResult AddToCart(string id, int SoLuong)
        {
            SanPham modelHome = new SanPham();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelHome = JsonConvert.DeserializeObject<SanPham>(jsonData);


            };


            var myCart = Giohangs;
            var item = myCart.SingleOrDefault(c => c.Maasp == id);

            if (item == null)
            {

                item = new CartItem
                {
                    Maasp = id,
                    Tensp = modelHome.TenSanPham,
                    DonGia = modelHome.GiaBan,
                    Soluong = SoLuong,


                };
                myCart.Add(item);



            }
            else if (myCart.Exists(c => c.Maasp == id))
            {
                item.Soluong += SoLuong;

            }
            else
            {
                item.Soluong++;
            }
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            var json = System.Text.Json.JsonSerializer.Serialize(myCart, opt);
            Response.Cookies.Append("Cart", json);


            return RedirectToAction("Index");
        }

        public IActionResult SuaSoLuong(string id, int soluongmoi)
        {
            //var sanPhams = ListData().SingleOrDefault(c => c.sanPhams.ID_SanPham == id);
            var myCart = Giohangs;
            var item = myCart.SingleOrDefault(c => c.Maasp == id);

            if (item != null)
            {

                item.Soluong = soluongmoi;
            }
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            var json = System.Text.Json.JsonSerializer.Serialize(myCart, opt);
            Response.Cookies.Append("Cart", json, new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(3)
            });
            return RedirectToAction("Index");


        }
        public IActionResult XoaKhoiGio(string id)
        {
            var myCart = Giohangs;
            var item = myCart.SingleOrDefault(c => c.Maasp == id);
            if (item != null)
            {
                myCart.Remove(item);

            }
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            var json = System.Text.Json.JsonSerializer.Serialize(myCart, opt);
            Response.Cookies.Append("Cart", json);
            return RedirectToAction("Index");
        }
    }
}
