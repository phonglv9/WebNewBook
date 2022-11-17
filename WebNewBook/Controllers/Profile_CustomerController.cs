using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
   
    public class Profile_CustomerController : Controller
    {
        //bảng điều khiển
        public async Task<IActionResult> account()
        {
            KhachHang khachHang = new KhachHang();
            List<HoaDon> lstOrder = new List<HoaDon>();
            HttpClient _httpClient = new HttpClient(); 
            string Id_khachang = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            HttpResponseMessage responseCustomer = _httpClient.GetAsync("https://localhost:7266/api/Customer/" + Id_khachang).Result;
            HttpResponseMessage responseOrder = _httpClient.GetAsync("https://localhost:7266/api/ProfileCustomer/GetOrder/" + Id_khachang).Result;
            if (responseCustomer.IsSuccessStatusCode)
            {
                string jsondata = responseCustomer.Content.ReadAsStringAsync().Result;
                khachHang = JsonConvert.DeserializeObject<KhachHang>(jsondata);
                ViewBag.KhachHang = khachHang;
            }
            if (responseOrder.IsSuccessStatusCode)
            {
                string jsondata = responseOrder.Content.ReadAsStringAsync().Result;
                lstOrder = JsonConvert.DeserializeObject<List<HoaDon>>(jsondata);
                ViewBag.lstOrder = lstOrder.Where(c => c.NgayMua.Year == DateTime.Now.Year);
                ViewBag.soLuongOrder = lstOrder.Where(c => c.NgayMua.Year == DateTime.Now.Year && c.TrangThai==1).Count();
                ViewBag.tongTienOrder = lstOrder.Where(c => c.NgayMua.Year == DateTime.Now.Year && c.TrangThai==1).Sum(c=>c.TongTien);
            }
          
          
            return View();
        }

      
        // màn show thông tin khách hàng 
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
        public  async Task<IActionResult> order()
        {
           
            List<HoaDon> lstOrder = new List<HoaDon>();
            HttpClient _httpClient = new HttpClient();
            string Id_khachang = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            HttpResponseMessage responseOrder = _httpClient.GetAsync("https://localhost:7266/api/ProfileCustomer/GetOrder/" + Id_khachang).Result;
            if (responseOrder.IsSuccessStatusCode)
            {
                string jsondata = responseOrder.Content.ReadAsStringAsync().Result;
                lstOrder = JsonConvert.DeserializeObject<List<HoaDon>>(jsondata);
                ViewBag.lstOrder = lstOrder;
               
            }
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

        public async Task<IActionResult> OrderDetail(string id)
        {
          
            HttpClient _httpClient = new HttpClient();
            List<HoaDonCT> lsthoaDonCTs = new List<HoaDonCT>();
            HoaDon hoaDon = new HoaDon();
            HttpResponseMessage responseOrderbyId = _httpClient.GetAsync("https://localhost:7266/api/ProfileCustomer/GetOrderById/" + id).Result;
            HttpResponseMessage responseOrderdetail = _httpClient.GetAsync("https://localhost:7266/api/ProfileCustomer/GetOrderdetail/" + id).Result;
            if (responseOrderdetail.IsSuccessStatusCode)
            {
                string jsondata = responseOrderdetail.Content.ReadAsStringAsync().Result;
                lsthoaDonCTs = JsonConvert.DeserializeObject<List<HoaDonCT>>(jsondata);
            }
            if (responseOrderbyId.IsSuccessStatusCode)
            {
                string jsondata = responseOrderbyId.Content.ReadAsStringAsync().Result;
                hoaDon = JsonConvert.DeserializeObject<HoaDon>(jsondata);
            }
            ViewBag.lstOrder = lsthoaDonCTs;
            ViewBag.hoadonById = hoaDon;
            return View();
        }
    }
}
