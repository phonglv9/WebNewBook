using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using WebNewBook.Model;
using WebNewBook.Models;

namespace WebNewBook.Controllers
{
    public class PaymentController : Controller
    {
        private readonly HttpClient _httpClient;
        public PaymentController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
        }
        public async Task<List<KhachHang>?> GetKhachHang()
        {
            List<KhachHang> khachHangs = new List<KhachHang>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("khachhang");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                khachHangs = JsonConvert.DeserializeObject<List<KhachHang>>(jsonData);
            };
            return khachHangs;
        }
        public async Task<List<SanPham>?> GetSanPham()
        {
            List<SanPham> sanPhams = new List<SanPham>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("khachhang");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sanPhams = JsonConvert.DeserializeObject<List<SanPham>>(jsonData);
            };
            return sanPhams;
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
        [HttpPost]
        public async Task<IActionResult>Pay(HoaDon hoaDon, string payment, string paymentnow)
        {
            try
            {
                List<KhachHang>? ListKhachHangs = new List<KhachHang>();
                ListKhachHangs = await GetKhachHang();
                var KhachHang = ListKhachHangs.Where(c => c.Email == User.Identity.Name).FirstOrDefault();
                if (payment == "1")
                {
                    var lstCart = Giohangs;

                   

                    foreach (var item in lstCart)
                    {
                        
                        hoaDon.ID_HoaDon = "HD" + Guid.NewGuid().ToString();
                        if (KhachHang ==null)

                        {
                            KhachHang = new KhachHang();
                            KhachHang.ID_KhachHang = "KH" + Guid.NewGuid().ToString();
                            KhachHang.HoVaTen = hoaDon.TenNguoiNhan;
                            KhachHang.SDT = hoaDon.SDT;
                            KhachHang.DiaChi = hoaDon.DiaChiGiaoHang;
                            KhachHang.Email = hoaDon.Email;
                            KhachHang.MatKhau = "123123";
                            KhachHang.TrangThai = 1;
                            KhachHang.DiemTichLuy = 1;
                            KhachHang.NgaySinh = DateTime.Now;
                            StringContent contentKH = new StringContent(JsonConvert.SerializeObject(KhachHang), Encoding.UTF8, "application/json");
                            HttpResponseMessage responseKH = await _httpClient.PostAsync("api/Customer", contentKH);

                        }
                        hoaDon.MaKhachHang = KhachHang.ID_KhachHang;                    
                        hoaDon.TongTien = item.ThanhTien;
                        hoaDon.NgayMua = DateTime.Now;
                        hoaDon.TrangThai = 2;
                        //hoaDon.MaGiamGia = "123";
                        StringContent contentHD = new StringContent(JsonConvert.SerializeObject(hoaDon), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseHD = await _httpClient.PostAsync("api/HoaDon/AddHoaDon", contentHD);
                       

                        List<HoaDonCT> listHoaDonCTs = new List<HoaDonCT>();
                        HoaDonCT hoaDonCT = new HoaDonCT();
                        hoaDonCT.ID_HDCT = Guid.NewGuid().ToString();
                        hoaDonCT.MaSanPham = item.Maasp;
                        hoaDonCT.MaHoaDon = hoaDon.ID_HoaDon;
                        hoaDonCT.SoLuong = item.Soluong;



                        listHoaDonCTs.Add(hoaDonCT);
                        StringContent contentHDCT = new StringContent(JsonConvert.SerializeObject(listHoaDonCTs), Encoding.UTF8, "application/json");
                        HttpResponseMessage responseHDCT = await _httpClient.PostAsync("api/HoaDon/AddHoaDonCT", contentHDCT);
                        ViewBag.SuccessMessage = "Thanh toán thành công";



                    }
                }

            }
            catch (Exception)
            {

                throw null;
            }
            
            
            return View();
        }
    }
}
