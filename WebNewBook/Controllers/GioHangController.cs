using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
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
            
         // Giohangs();

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
      
        

        public async Task<IActionResult> Index(string? mess)
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
        public List<CartItem> Giohangs()
        {
            List<CartItem> data = new List<CartItem>();
            if (User.Identity.Name == null)
            {

               

                var jsonData = Request.Cookies["Cart"];
                if (jsonData != null)
                {
                    data = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
                }
                else if (data == null)
                {
                   
                    return data;



                }
            }
            else
            {
                List<GioHang> gioHangs = new List<GioHang>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/GioHang/GetLitsGH").Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonData1 = response.Content.ReadAsStringAsync().Result;
                    gioHangs = JsonConvert.DeserializeObject<List<GioHang>>(jsonData1);
                    foreach(var a in gioHangs)
                    {
                        CartItem b=new CartItem();
                        if (User.Identity.Name == a.emailKH)
                        {
                            b.DonGia = a.DonGia;
                            b.Maasp = a.Maasp;
                            b.Soluong = a.Soluong;
                            b.Tensp = a.Tensp;
                            b.ThanhTien = a.Soluong * a.DonGia;
                             data.Add(b);
                           

                        }
                    }
                    return data;

                };


            }

            return data;

        }

        //[HttpPost]
        //public async Task<IActionResult> CreateGioHang()
        //{
        //    if (!string.IsNullOrEmpty(User.Identity.Name))
        //    {
        //        List<GioHang> lstGioHang = new List<GioHang>();
        //        GioHang GioHangs = new GioHang();
        //        foreach (var a in Giohangs())
        //        {
                    

        //            GioHangs.ID_GioHang = "GH" + Guid.NewGuid().ToString();
        //            GioHangs.HinhAnh = "gsdfgsdg";
        //            GioHangs.TenSP = a.Tensp;
        //            GioHangs.DonGia = a.DonGia;
        //            GioHangs.SoLuong = a.Soluong;
        //            GioHangs.TrangThai = true;
        //            GioHangs.TongDonGia = a.ThanhTien;
        //            GioHangs.emailKH = User.Identity.Name;
        //            lstGioHang.Add(GioHangs);
        //        }

        //        GioHang modelHome = new GioHang();
        //        HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/GioHang/Addgiohang"+GioHangs).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string jsonData = response.Content.ReadAsStringAsync().Result;
        //            modelHome = JsonConvert.DeserializeObject<GioHang>(jsonData);


        //        };
        //    }


        //    return View("");
        //}


        public IActionResult AddToCart(string id, int SoLuong)
           {
            if (User.Identity.Name == null)
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
                        ThanhTien = SoLuong * modelHome.GiaBan


                    };
                    myCart.Add(item);



                }
                else if (myCart.Exists(c => c.Maasp == id))
                {
                    if (SoLuong >= modelHome.SoLuong || item.Soluong >= modelHome.SoLuong)
                    {

                        return Json("Số lượng không có sẵn");
                    }
                    else
                    {
                        item.Soluong += SoLuong;
                        item.ThanhTien = item.Soluong * item.DonGia;

                    }


                }
                else
                {
                    item.Soluong++;
                    item.ThanhTien = item.Soluong * item.DonGia;
                }
                var opt = new CookieOptions() { Expires = new DateTimeOffset(DateTime.Now.AddDays(3)) };

                var json = System.Text.Json.JsonSerializer.Serialize(myCart);
                Response.Cookies.Append("Cart", json, opt);


               
            }
            else
            {
             
              
                SanPham modelHome = new SanPham();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = response.Content.ReadAsStringAsync().Result;
                    modelHome = JsonConvert.DeserializeObject<SanPham>(jsonData);


                };

                string HinhAnh = "dsfgsdfg";
                int SoLuongs = SoLuong;
                string emailKH = User.Identity.Name;
                string idsp = id;
               
               

                HttpResponseMessage response1 = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Addgiohang/{HinhAnh}/{SoLuongs}/{emailKH}/{idsp}" ).Result;
               



            }
            return Json("Thêm vào giỏ hàng thành công");
        }

        public IActionResult SuaSoLuong(string id, int soluongmoi, string update)
        {
            if (User.Identity.Name==null)
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
                            item.ThanhTien = item.Soluong * item.DonGia;
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
                            item.ThanhTien = item.Soluong * item.DonGia;
                            if (item.Soluong  > sanPham.SoLuong)
                            {
                                item.Soluong = item.Soluong - 1;
                                item.ThanhTien = item.Soluong * item.DonGia;
                                return RedirectToAction("Index", new { mess = "Số lượng không có sẵn" });
                            }
                        }
                        else
                        {
                            item.Soluong = item.Soluong - 1;
                            item.ThanhTien = item.Soluong * item.DonGia;
                            if (item.Soluong <= 0)
                            {

                                item.Soluong = 1;
                                item.ThanhTien = item.Soluong * item.DonGia;
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

            }
            else
            {
                int mess = 0;
                string thongbao = "";
                string namekh = User.Identity.Name;
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Updatenumber/{id}/{soluongmoi}/{namekh}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = response.Content.ReadAsStringAsync().Result;
                    mess = JsonConvert.DeserializeObject<int>(jsonData);
                    if(mess == 1)
                    {
                        thongbao= "Đã được cập nhật";
                    }
                    else if(mess == 2)
                    {
                        thongbao = "Số lượng" + soluongmoi + "không có sẵn";

                    }
                    else
                    {
                        thongbao = "lỗi";

                    }

                    return RedirectToAction("Index", new { mess = thongbao });

                };
            }
            return RedirectToAction("Index");


        }
        //public IActionResult SuaSoLuong2(string id, int soLuong)
        //{
        //    SanPham sanPham = new SanPham();
        //    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonData = response.Content.ReadAsStringAsync().Result;
        //        sanPham = JsonConvert.DeserializeObject<SanPham>(jsonData);


        //    };

        //    var myCart = Giohangs();
        //    var item = myCart.SingleOrDefault(c => c.Maasp == id);
        //    if (item != null)
        //    {




        //    }


        //    var json = System.Text.Json.JsonSerializer.Serialize(myCart);
        //    Response.Cookies.Append("Cart", json, new Microsoft.AspNetCore.Http.CookieOptions
        //    {
        //        Expires = DateTimeOffset.Now.AddDays(3)
        //    });

        //    return RedirectToAction("Index");


        //}
        public IActionResult XoaKhoiGio(string id)
        {
            if(User.Identity.Name == null)
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
            else
            {
                var namekh = User.Identity.Name;
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Xoakhoigio/{id}/{namekh}").Result;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
    }
}
