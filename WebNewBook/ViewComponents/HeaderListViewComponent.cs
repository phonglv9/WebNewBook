using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Models;
using WebNewBook.Services;

namespace WebNewBook.Component
{

    public class HeaderListViewComponent : ViewComponent
    {
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;


        private IHeaderService _headerService;
        public HeaderListViewComponent(IHeaderService headerService)
        {
            _headerService = headerService;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headers = await _headerService.GetDMAsync();
            ViewBag.NavBar = headers;
            if (User.Identity.Name == null)
            {

                List<CartItem> data = new List<CartItem>();
                var jsonData = Request.Cookies["Cart"];
                if (jsonData != null)
                {
                    data = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
                }
                var b = data.Where(n => n.Tensp != null).Select(n => n.Tensp).ToList();
                var tongTien = data.Sum(a => a.ThanhTien);

                ViewBag.thanhtien = tongTien;
                ViewBag.soluong = data.Sum(a => a.Soluong);
                ViewBag.count = b.Count;
                ViewBag.giohang = data;
                HttpContext.Session.SetString("amout", tongTien.ToString());


            }
            else
            {


                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/GioHang/GetLitsGH").Result;
                if (response.IsSuccessStatusCode)
                {
                    List<CartItem> dataGHPoup = new List<CartItem>();
                    List<ModelCart> dataGH = new List<ModelCart>();
                    string jsonData1 = response.Content.ReadAsStringAsync().Result;
                    dataGH = JsonConvert.DeserializeObject<List<ModelCart>>(jsonData1);
                    if (dataGH.Where(c => c.EmailKH == User.Identity.Name).Select(v => v.EmailKH).FirstOrDefault() == User.Identity.Name)
                    {

                        foreach (var a in dataGH)
                        {
                            CartItem c = new CartItem();
                            if (User.Identity.Name == a.EmailKH)
                            {
                                c.DonGia = a.GiaBan;
                                c.Maasp = a.MaSanPham;
                                c.Soluong = a.SoLuong;
                                c.Tensp = a.TenSanPham;
                                c.hinhanh = a.HinhAnh;
                                c.ThanhTien = a.SoLuong * a.GiaBan;
                                dataGHPoup.Add(c);


                            }
                        }
                        var b = dataGHPoup.Where(n => n.Tensp != null).Select(n => n.Tensp).ToList();
                        var tongTien = dataGHPoup.Sum(a => a.Soluong * a.DonGia);
                        ViewBag.thanhtien = tongTien;
                        ViewBag.soluong = dataGHPoup.Sum(a => a.Soluong);
                        ViewBag.count = b.Count;
                        ViewBag.giohang = dataGHPoup;
                        HttpContext.Session.SetString("amout", tongTien.ToString());

                    }
                    else
                    {

                        List<CartItem> data = new List<CartItem>();
                        var jsonData = Request.Cookies["moi"];
                        if (jsonData != null)
                        {
                            data = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
                        }
                        var b = data.Where(n => n.Tensp != null).Select(n => n.Tensp).ToList();
                        var tongTien = data.Sum(a => a.ThanhTien);

                        ViewBag.thanhtien = tongTien;
                        ViewBag.soluong = data.Sum(a => a.Soluong);
                        ViewBag.count = b.Count;
                        ViewBag.giohang = data;
                        HttpContext.Session.SetString("amout", tongTien.ToString());

                    }


                };
            }
            return View();


        }

    }
}
