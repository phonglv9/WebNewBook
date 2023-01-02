using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Models;

namespace WebNewBook.Controllers
{

    //public class GioHangController : Controller
    //{
    //    Uri baseAdress = new Uri("https://localhost:7266/api");
    //    HttpClient _httpClient;

    //    public GioHangController()
    //    {
    //        _httpClient = new HttpClient();
    //        _httpClient.BaseAddress = baseAdress;

         
            

    //    }



      
      
        

    //    public async Task<IActionResult> Index(string? mess)
    //    {

           

    //       if(User.Identity.Name == null)
    //        {
               
    //            if (Giohangs() != null)
    //            {
    //                foreach (var item in Giohangs())
    //                {
    //                    var a = 0;
    //                    HttpResponseMessage response1 = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/GetSanPham/{item.Maasp}").Result;
    //                    if (response1.IsSuccessStatusCode)
    //                    {
    //                        string jsonData = response1.Content.ReadAsStringAsync().Result;
    //                        a = JsonConvert.DeserializeObject<int>(jsonData);


    //                    };




    //                    if (item.Soluong > a)
    //                    {
    //                        //var myCart = Giohangs();
    //                        //var items = myCart.SingleOrDefault(c => c.Maasp == id);

    //                        item.Soluong = a;
                           

    //                        var json = System.Text.Json.JsonSerializer.Serialize(Giohangs());
    //                        Response.Cookies.Append("Cart", json, new Microsoft.AspNetCore.Http.CookieOptions
    //                        {
    //                            Expires = DateTimeOffset.Now.AddDays(-1)
    //                        });
    //                        return RedirectToAction("Index" ,new { mess = "4" });

    //                    }
                        


    //                }
                    
    //            }
               
    //        }
    //        else
    //        {
    //            var a = 0;
    //            HttpResponseMessage response1 = _httpClient.GetAsync(_httpClient.BaseAddress + "/GioHang/ChecksoluongCart").Result;
    //            if (response1.IsSuccessStatusCode)
    //            {
    //                string jsonData = response1.Content.ReadAsStringAsync().Result;
    //                a = JsonConvert.DeserializeObject<int>(jsonData);

    //                if (a != 0)
    //                {
    //                    mess = a.ToString();
    //                }

    //            };
    //            Giohangs();
    //        }


    //        Giohangs();

    //        ViewBag.MessUpdateCart = mess;
    //        var cart = Giohangs();
    //        var tongTien = cart.Sum(a => a.ThanhTien);
            
    //        ViewBag.giohang = cart;
    //        ViewBag.thanhtien = tongTien;
    //        ViewBag.soluong = cart.Sum(a => a.Soluong);
    //        HttpContext.Session.SetString("amout", tongTien.ToString());
            

    //        return View("Index");
    //    }
    //    public List<CartItem> Giohangs()
    //    {
            
            
    //        List<CartItem> data = new List<CartItem>();
    //        if (User.Identity.Name == null)
    //        {

               

    //            var jsonData = Request.Cookies["Cart"];
    //            if (jsonData != null)
    //            {
    //                data = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
    //            }

    //            else if (data == null)
    //            {
                   
    //                return data;



    //            }
                
    //        }
    //        else
    //        {
    //            List<GioHang> gioHangs = new List<GioHang>();
    //            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/GioHang/GetLitsGH").Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                string jsonData1 = response.Content.ReadAsStringAsync().Result;
    //                gioHangs = JsonConvert.DeserializeObject<List<GioHang>>(jsonData1);
    //                foreach(var a in gioHangs)
    //                {
    //                    CartItem b=new CartItem();
    //                    if (User.Identity.Name == a.emailKH)
    //                    {
    //                        b.DonGia = a.DonGia;
    //                        b.Maasp = a.Maasp;
    //                        b.Soluong = a.Soluong;
    //                        b.Tensp = a.Tensp;
    //                        b.hinhanh = a.HinhAnh;
    //                        b.ThanhTien = a.Soluong * a.DonGia;
    //                         data.Add(b);
                           

    //                    }
    //                }
    //                return data;

    //            };


    //        }

    //        return data;

    //    }

       

    //    public IActionResult AddToCartCT(string id, int SoLuong)
    //    {
    //        /////

    //        if (User.Identity.Name == null)
    //        {
    //            SanPham modelHome = new SanPham();
    //            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                string jsonData = response.Content.ReadAsStringAsync().Result;
    //                modelHome = JsonConvert.DeserializeObject<SanPham>(jsonData);


    //            };


    //            var myCart = Giohangs();
    //            var item = myCart.SingleOrDefault(c => c.Maasp == id);



    //            if (item == null)
    //            {

    //                item = new CartItem
    //                {
    //                    Maasp = id,
    //                    Tensp = modelHome.TenSanPham,
    //                    DonGia = modelHome.GiaBan,
    //                    Soluong = SoLuong,
    //                    hinhanh=modelHome.HinhAnh,
    //                    ThanhTien = SoLuong * modelHome.GiaBan
                        

    //                };
    //                myCart.Add(item);


    //                if (SoLuong > modelHome.SoLuong)
    //                {

    //                    return RedirectToAction("Index", new { mess = 2 });
    //                }



    //            }
    //             else if (myCart.Exists(c => c.Maasp == id))
    //            {
    //                    if (SoLuong + item.Soluong > modelHome.SoLuong || item.Soluong > modelHome.SoLuong)
    //                    {

    //                        return RedirectToAction("Index", new { mess = 2 });
    //                    }
                       
    //                    else
    //                    {
    //                        item.Soluong += SoLuong;
    //                        item.ThanhTien = item.Soluong * item.DonGia;

    //                    }
                        
                   
                    


    //            }
    //            else
    //            {
    //                item.Soluong++;
    //                item.ThanhTien = item.Soluong * item.DonGia;
    //            }
    //            var opt = new CookieOptions() { Expires = new DateTimeOffset(DateTime.Now.AddDays(3)) };

    //            var json = System.Text.Json.JsonSerializer.Serialize(myCart);
    //            Response.Cookies.Append("Cart", json, opt);



    //        }
    //        else
    //        {


    //            SanPham modelHome = new SanPham();
    //            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                string jsonData = response.Content.ReadAsStringAsync().Result;
    //                modelHome = JsonConvert.DeserializeObject<SanPham>(jsonData);


    //            };

    //            string HinhAnh = modelHome.HinhAnh;
    //            int SoLuongs = SoLuong;
    //            string emailKH = User.Identity.Name;
    //            string idsp = id;


    //            int mess = 0;
               
    //            HttpResponseMessage response1 = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Addgiohang/{HinhAnh}/{SoLuongs}/{emailKH}/{idsp}").Result;
    //            if (response1.IsSuccessStatusCode)
    //            {
    //                string jsonData = response1.Content.ReadAsStringAsync().Result;
    //                mess = JsonConvert.DeserializeObject<int>(jsonData);
    //                if (mess == 3)
    //                {
    //                    return RedirectToAction("Index");
    //                }

                   
    //                return RedirectToAction("Index", new { mess = mess });
    //            };
              



    //        }
    //        return RedirectToAction("Index");
    //    }
    //    public IActionResult AddToCart(string id, int SoLuong)
    //       {
    //        if (!string.IsNullOrEmpty(id))
    //        {
    //            if (User.Identity.Name == null)
    //            {
    //                SanPham modelHome = new SanPham();
    //                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    string jsonData = response.Content.ReadAsStringAsync().Result;
    //                    modelHome = JsonConvert.DeserializeObject<SanPham>(jsonData);


    //                };


    //                var myCart = Giohangs();
    //                var item = myCart.SingleOrDefault(c => c.Maasp == id);

    //                if (item == null)
    //                {

    //                    item = new CartItem
    //                    {
    //                        Maasp = id,
    //                        Tensp = modelHome.TenSanPham,
    //                        DonGia = modelHome.GiaBan,
    //                        Soluong = SoLuong,
    //                        hinhanh = modelHome.HinhAnh,
    //                        ThanhTien = SoLuong * modelHome.GiaBan


    //                    };
    //                    myCart.Add(item);



    //                }
    //                else if (myCart.Exists(c => c.Maasp == id))
    //                {
    //                    if (SoLuong + item.Soluong > modelHome.SoLuong || item.Soluong > modelHome.SoLuong)
    //                    {

    //                        return Json("Số lượng không có sẵn");
    //                    }
                    
    //                    else
    //                    {
    //                        item.Soluong += SoLuong;
    //                        item.ThanhTien = item.Soluong * item.DonGia;

    //                    }


    //                }
    //                else
    //                {
    //                    item.Soluong++;
    //                    item.ThanhTien = item.Soluong * item.DonGia;
    //                }
    //                var opt = new CookieOptions() { Expires = new DateTimeOffset(DateTime.Now.AddDays(3)) };

    //                var json = System.Text.Json.JsonSerializer.Serialize(myCart);
    //                Response.Cookies.Append("Cart", json, opt);



    //            }
    //            else
    //             {


    //                SanPham modelHome = new SanPham();
    //                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    string jsonData = response.Content.ReadAsStringAsync().Result;
    //                    modelHome = JsonConvert.DeserializeObject<SanPham>(jsonData);


    //                };

    //                string HinhAnh = modelHome.HinhAnh;
    //                int SoLuongs = SoLuong;
    //                string emailKH = User.Identity.Name;
    //                string idsp = id;


    //                int mess = 0;

    //                HttpResponseMessage response1 = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Addgiohang/{HinhAnh}/{SoLuongs}/{emailKH}/{idsp}").Result;
    //                if (response1.IsSuccessStatusCode)
    //                {
    //                    string jsonData = response1.Content.ReadAsStringAsync().Result;
    //                    mess = JsonConvert.DeserializeObject<int>(jsonData);
    //                     if(mess == 2)
    //                    {
    //                        return Json("Số lượng không có sẵn");
    //                    }

                       
    //                };


    //            }
    //        }
    //        return Json("Thêm vào giỏ hàng thành công");
    //    }

    //    public IActionResult SuaSoLuong(string id, int soluongmoi, string update)
    //    {
    //        if (User.Identity.Name==null)
    //        {
    //            SanPham sanPham = new SanPham();
    //            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/SanPham/{id}").Result;
    //            if (response.IsSuccessStatusCode)
    //            {
    //                string jsonData = response.Content.ReadAsStringAsync().Result;
    //                sanPham = JsonConvert.DeserializeObject<SanPham>(jsonData);


    //            };

    //            var myCart = Giohangs();
    //            var item = myCart.SingleOrDefault(c => c.Maasp == id);
    //            if (item != null)
    //            {
    //                if (soluongmoi != 0)
    //                {
    //                    if (soluongmoi <= sanPham.SoLuong)
    //                    {
    //                        item.Soluong = soluongmoi;
    //                        item.ThanhTien = item.Soluong * item.DonGia;
    //                    }
    //                    else
    //                    {
    //                        return RedirectToAction("Index", new { mess =2 });
    //                    }
    //                }
    //                else
    //                {
    //                    if (update == "1")
    //                    {
    //                        item.Soluong = item.Soluong + 1;
    //                        item.ThanhTien = item.Soluong * item.DonGia;
    //                        if (item.Soluong +1  > sanPham.SoLuong)
    //                        {
    //                            item.Soluong = item.Soluong - 1;
    //                            item.ThanhTien = item.Soluong * item.DonGia;
    //                            return RedirectToAction("Index", new { mess =2 });
    //                        }
    //                    }
    //                    else
    //                    {
    //                        item.Soluong = item.Soluong - 1;
    //                        item.ThanhTien = item.Soluong * item.DonGia;
    //                        if (item.Soluong <= 0)
    //                        {
    //                            var myCartt = Giohangs();
    //                            var item1 = myCartt.SingleOrDefault(c => c.Maasp == id);
    //                            if (item1 != null)
    //                            {
    //                                myCart.Remove(item);

    //                            }

    //                            var jsonn = System.Text.Json.JsonSerializer.Serialize(myCart);
    //                            Response.Cookies.Append("Cart", jsonn);
    //                            return RedirectToAction("Index");

    //                        }
    //                    }

    //                }





    //            }


    //            var json = System.Text.Json.JsonSerializer.Serialize(myCart);
    //            Response.Cookies.Append("Cart", json, new Microsoft.AspNetCore.Http.CookieOptions
    //            {
    //                Expires = DateTimeOffset.Now.AddDays(3)
    //            });
    //            return RedirectToAction("Index", new { mess = 3 });

    //        }
    //        else
    //        {
    //            if (soluongmoi != 0) {
    //                int mess = 0;
    //                update = "0";
    //                string namekh = User.Identity.Name;
    //                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Updatenumber/{id}/{soluongmoi}/{namekh}/{update}").Result;
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    string jsonData = response.Content.ReadAsStringAsync().Result;
    //                    mess = JsonConvert.DeserializeObject<int>(jsonData);
                       

    //                    return RedirectToAction("Index", new { mess = mess });

    //                };
    //            }
    //            else
    //            {
    //                int mess = 0;
    //                string namekh = User.Identity.Name;
    //                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Updatenumber/{id}/{soluongmoi}/{namekh}/{update}").Result;
    //                if (response.IsSuccessStatusCode)
    //                {
    //                    string jsonData = response.Content.ReadAsStringAsync().Result;
    //                    mess = JsonConvert.DeserializeObject<int>(jsonData);


    //                    return RedirectToAction("Index", new { mess = mess });

    //                };
    //            }

             
    //        }
    //        return RedirectToAction("Index");


    //    }
      
    //    public IActionResult XoaKhoiGio(string id)
    //    {
    //        if(User.Identity.Name == null)
    //        {
    //            var myCart = Giohangs();
    //            var item = myCart.SingleOrDefault(c => c.Maasp == id);
    //            if (item != null)
    //            {
    //                myCart.Remove(item);

    //            }

    //            var json = System.Text.Json.JsonSerializer.Serialize(myCart);
    //            Response.Cookies.Append("Cart", json);
    //            return RedirectToAction("Index");

    //        }
    //        else
    //        {
    //            var namekh = User.Identity.Name;
    //            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Xoakhoigio/{id}/{namekh}").Result;
    //            return RedirectToAction("Index");
    //        }
           

    //    }
    //    public IActionResult XoaKhoiGioHome(string id)
    //    {
    //        if (User.Identity.Name == null)
    //        {
    //            var myCart = Giohangs();
    //            var item = myCart.SingleOrDefault(c => c.Maasp == id);
    //            if (item != null)
    //            {
    //                myCart.Remove(item);

    //            }

    //            var json = System.Text.Json.JsonSerializer.Serialize(myCart);
    //            Response.Cookies.Append("Cart", json);
    //            return RedirectToAction("Index");

    //        }
    //        else
    //        {
    //            var namekh = User.Identity.Name;
    //            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/GioHang/Xoakhoigio/{id}/{namekh}").Result;
    //            return RedirectToAction("Index");
    //        }


    //    }
    //}
}
