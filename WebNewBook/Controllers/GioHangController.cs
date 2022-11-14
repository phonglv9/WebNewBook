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

            //ListData();

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

        public List<CartItem> Giohangs()
        {
            
                List<CartItem> data = new List<CartItem>();

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

        public async Task<IActionResult> Index(string?mess)
        {
            ViewBag.MessUpdateCart = mess;
            var cart = Giohangs();
            var tongTien = cart.Sum(a => a.ThanhTien);
            ViewBag.giohang = cart;
            ViewBag.thanhtien = tongTien;
            ViewBag.soluong = cart.Sum(a => a.Soluong);
            HttpContext.Session.SetString("amout", tongTien.ToString());
            
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


            var myCart = Giohangs();
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
                if (SoLuong > modelHome.SoLuong || item.Soluong > modelHome.SoLuong)
                {
                    
                    return Json("Số lượng không có sẵn");
                }
                else
                {
                    item.Soluong += SoLuong;

                }


            }
            else
            {
                item.Soluong++;
            }
            var opt = new CookieOptions() {  Expires =  new DateTimeOffset(DateTime.Now.AddDays(3))};
           
            var json = System.Text.Json.JsonSerializer.Serialize(myCart);
            Response.Cookies.Append("Cart", json, opt);

           
            return Json("Thêm vào giỏ hàng thành công");
        }

        public IActionResult SuaSoLuong(string id, int soluongmoi,string update)
        {
            SanPham sanPham = new SanPham();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                sanPham = JsonConvert.DeserializeObject<SanPham>(jsonData);


            };
           
            var myCart = Giohangs();
            var item = myCart.SingleOrDefault(c => c.Maasp == id);
            if (item != null)
            {
                if (soluongmoi != 0)
                {
                    if (soluongmoi <= sanPham.SoLuong)
                    {
                        item.Soluong = soluongmoi;
                    }
                    else
                    {
                        return RedirectToAction("Index", new { mess = "Số lượng" + soluongmoi + "không có sẵn" });
                    }
                }
                else
                {
                    if (update == "1")
                    {
                        item.Soluong = item.Soluong + 1;
                        if (item.Soluong > sanPham.SoLuong)
                        {
                            item.Soluong =  item.Soluong - 1;
                            return RedirectToAction("Index", new { mess = "Số lượng không có sẵn" });
                        }
                    }
                    else
                    {
                        item.Soluong = item.Soluong -1;
                        if (item.Soluong <= 0 )
                        {
                            item.Soluong = 1;
                            return RedirectToAction("Index");
                        }
                    }

                }
                    
                



            }


            var json = System.Text.Json.JsonSerializer.Serialize(myCart);
            Response.Cookies.Append("Cart", json, new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(3)
            });

            return RedirectToAction("Index");


        }
        public IActionResult SuaSoLuong2(string id, int soLuong)
        {
            SanPham sanPham = new SanPham();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                sanPham = JsonConvert.DeserializeObject<SanPham>(jsonData);


            };

            var myCart = Giohangs();
            var item = myCart.SingleOrDefault(c => c.Maasp == id);
            if (item != null)
            {                           
                   
               


            }


            var json = System.Text.Json.JsonSerializer.Serialize(myCart);
            Response.Cookies.Append("Cart", json, new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(3)
            });

            return RedirectToAction("Index");


        }
        public IActionResult XoaKhoiGio(string id)
        {
            var myCart = Giohangs();
            var item = myCart.SingleOrDefault(c => c.Maasp == id);
            if (item != null)
            {
                myCart.Remove(item);

            }
          
            var json = System.Text.Json.JsonSerializer.Serialize(myCart);
            Response.Cookies.Append("Cart", json);
            return RedirectToAction("Index");
        }
    }
}
