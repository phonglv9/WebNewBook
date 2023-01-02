using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.ViewModel;

namespace WebNewBook.Controllers
{
   
    public class Profile_CustomerController : Controller
    {


        Uri link = new Uri("https://localhost:7266/api");
        HttpClient client;
        public Profile_CustomerController()
        {
            client = new HttpClient();
            client.BaseAddress = link;
          
        }

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
                ViewBag.soLuongOrder = lstOrder.Where(c => c.NgayMua.Year == DateTime.Now.Year && c.TrangThai==5).Count();
                ViewBag.tongTienOrder = lstOrder.Where(c => c.NgayMua.Year == DateTime.Now.Year && c.TrangThai==5).Sum(c=>c.TongTien);
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
            KhachHang khachHang = new KhachHang();
            HttpClient _httpClient = new HttpClient();
            string Id_khachang = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            HttpResponseMessage response1 = _httpClient.GetAsync("https://localhost:7266/api/Customer/" + Id_khachang).Result;

            if (response1.IsSuccessStatusCode)
            {
                string jsondata = response1.Content.ReadAsStringAsync().Result;
                khachHang = JsonConvert.DeserializeObject<KhachHang>(jsondata);
                ViewBag.diemtichluy = khachHang.DiemTichLuy;
            }


            List<Voucher> lstvoucher = new List<Voucher>();
            HttpResponseMessage response2 = _httpClient.GetAsync("https://localhost:7266/api/VouCher/").Result;

            if (response2.IsSuccessStatusCode)
            {
                string jsondata = response2.Content.ReadAsStringAsync().Result;
                lstvoucher = JsonConvert.DeserializeObject<List<Voucher>>(jsondata);
                var dateNow = DateTime.Now;
                ViewBag.VoucherPhatHanh = lstvoucher.Where(c => c.HinhThuc == 1 &&  c.SoLuong>0 && c.TrangThai==1 && dateNow >= c.StartDate && dateNow <= c.EndDate);
            }

            List<VoucherCT> lstvoucherCT = new List<VoucherCT>();
            HttpResponseMessage response3 = _httpClient.GetAsync("https://localhost:7266/api/VoucherCT/VoucherKH/"+Id_khachang).Result;

            if (response3.IsSuccessStatusCode)
            {
                string jsondata = response3.Content.ReadAsStringAsync().Result;
                lstvoucherCT = JsonConvert.DeserializeObject<List<VoucherCT>>(jsondata);
                ViewBag.VoucherByCuster = lstvoucherCT.Where(c=>c.TrangThai!=2);
            }

            List<VoucherPaymentVM> voucherPaymentVMs = new List<VoucherPaymentVM>();
            foreach (var Vouchers in lstvoucher)
            {
                foreach (var VoucherCTs in lstvoucherCT.Where(c => c.MaVoucher == Vouchers.Id && c.TrangThai!=2))
                {

                    VoucherPaymentVM VoucherPayment = new VoucherPaymentVM();
                    VoucherPayment.ID_Voucher = VoucherCTs.Id;
                    VoucherPayment.TenPhatHanh = Vouchers.TenPhatHanh;
                    VoucherPayment.MenhGia = Vouchers.MenhGia;
                    VoucherPayment.MenhGiaDieuKien = Vouchers.MenhGiaDieuKien;
                    VoucherPayment.NgayBatDau = VoucherCTs.NgayBatDau;
                    VoucherPayment.NgayHetHan = VoucherCTs.NgayHetHan;
                    voucherPaymentVMs.Add(VoucherPayment);
                }

            }
            ViewBag.ListVoucher = voucherPaymentVMs;


            return View();
        }

        public IActionResult DoiVoucher(string id)
        {

            HttpClient _httpClient = new HttpClient();
            string Id_khachang = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;



            HttpResponseMessage response = _httpClient.PutAsync("https://localhost:7266/api/VoucherCT/DoiVoucherAccount?maph=" + id+"&makh="+ Id_khachang, null).Result;
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("VoucherWallet", new { mess = 1 });
            }
            else
            {

                return RedirectToAction("VoucherWallet", new { mess = 2 });
            }

        }


        public IActionResult FpointHistory()
        {
            List<Fpoint> lstfpoints = new List<Fpoint>();
            HttpClient _httpClient = new HttpClient();
            string Id_khachang = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            HttpResponseMessage response = _httpClient.GetAsync("https://localhost:7266/api/Fpoint").Result;

            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                lstfpoints = JsonConvert.DeserializeObject<List<Fpoint>>(jsondata);
            }

            ViewBag.lstfpoin = lstfpoints.Where(c=>c.MaKhachHang==Id_khachang).OrderByDescending(c=>c.CreatDate);


            KhachHang khachHang = new KhachHang();
            HttpResponseMessage response_khachang = _httpClient.GetAsync("https://localhost:7266/api/Customer/" + Id_khachang).Result;

            if (response_khachang.IsSuccessStatusCode)
            {
                string jsondata1 = response_khachang.Content.ReadAsStringAsync().Result;
                khachHang = JsonConvert.DeserializeObject<KhachHang>(jsondata1);
            }

            ViewBag.Diemtichluy = khachHang.DiemTichLuy;


            return View();
        }


        public async Task<IActionResult> OrderDetail(string id)
        {

            //double thanhtien=0;
         
            //HttpClient _httpClient = new HttpClient();
            //List<HoaDonCT> lsthoaDonCTs = new List<HoaDonCT>();
            //List<ViewHoaDon> lstviewHoaDons= new List<ViewHoaDon>();
            //HoaDon hoaDon = new HoaDon();
            //HttpResponseMessage responseOrderbyId = _httpClient.GetAsync("https://localhost:7266/api/ProfileCustomer/GetOrderById/" + id).Result;
            //HttpResponseMessage responseOrderdetail = _httpClient.GetAsync("https://localhost:7266/api/ProfileCustomer/GetOrderdetail/" + id).Result;
            //HttpResponseMessage responseListOrderdetail = _httpClient.GetAsync("https://localhost:7266/api/HoaDon/getlistid/" + id).Result;
            //if (responseOrderdetail.IsSuccessStatusCode)
            //{
            //    string jsondata = responseOrderdetail.Content.ReadAsStringAsync().Result;
            //    lsthoaDonCTs = JsonConvert.DeserializeObject<List<HoaDonCT>>(jsondata);
            //}
            //if (responseOrderbyId.IsSuccessStatusCode)
            //{
            //    string jsondata = responseOrderbyId.Content.ReadAsStringAsync().Result;
            //    hoaDon = JsonConvert.DeserializeObject<HoaDon>(jsondata);
            //}
            //if (responseListOrderdetail.IsSuccessStatusCode)
            //{
            //    string jsondata = responseListOrderdetail.Content.ReadAsStringAsync().Result;
            //    lstviewHoaDons = JsonConvert.DeserializeObject<List<ViewHoaDon>>(jsondata);
            //}
            //ViewBag.lstOrder = lsthoaDonCTs;
            //ViewBag.hoadonById = hoaDon;
            //ViewBag.lstviewHoadon = lstviewHoaDons;
            


            //foreach (var x in lstviewHoaDons.Select(c => c.hoaDonCT))
            //{
            //    thanhtien += x.GiaBan * x.SoLuong;
              
            //}
            //ViewBag.thanhtien = thanhtien;
            //ViewBag.tongtien = lstviewHoaDons.FirstOrDefault(c => c.hoaDon.ID_HoaDon == id).hoaDon.TongTien;
            //ViewBag.chietkhau =( ViewBag.thanhtien) - (ViewBag.tongtien);
            //return View();

            ViewBag.TitleAdmin = "Chi tiết hóa đơn";
            List<ViewHoaDonCT> lissttlhdct = new List<ViewHoaDonCT>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/HoaDon/getlistid/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttlhdct = JsonConvert.DeserializeObject<List<ViewHoaDonCT>>(data);
                //Thông tin khách hàng
                ViewBag.IDLogin = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.ID_KhachHang).FirstOrDefault();
                ViewBag.NameLogin = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.HoVaTen).FirstOrDefault();
                ViewBag.SDTLogin = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.SDT).FirstOrDefault();
                ViewBag.EmailLogin = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.Email).FirstOrDefault();

                var hoaDon = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).FirstOrDefault();
                Voucher voucher = new Voucher();
                HttpResponseMessage responsevc = client.GetAsync(client.BaseAddress + $"/HoaDon/GetPriceVoucher/{hoaDon.hoaDon.MaGiamGia}").Result;
                if (responsevc.IsSuccessStatusCode)
                {
                    string data2 = responsevc.Content.ReadAsStringAsync().Result;
                    voucher = JsonConvert.DeserializeObject<Voucher>(data2);
                    ViewBag.PriceVoucher = voucher.MenhGia;
                   
                }

                //Thông tin hóa đơn
                ViewBag.IdHoaDon = id;
                ViewBag.Namekh = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.TenNguoiNhan).FirstOrDefault();
                ViewBag.sdtkh = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.SDT).FirstOrDefault();
                ViewBag.ghichu = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.GhiChu).FirstOrDefault();
                ViewBag.diachi = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.DiaChiGiaoHang).FirstOrDefault();
                ViewBag.ngaymua = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.NgayMua).FirstOrDefault();
                ViewBag.tongtien = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.TongTien).FirstOrDefault();
                ViewBag.trangThai = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.TrangThai).FirstOrDefault();
                ViewBag.thanhTien = ViewBag.tongtien + voucher.MenhGia;
            }




            return View("OrderDetail", lissttlhdct);



        }
        [HttpPost]
        public int UpdateAccount(KhachHang khachHang)
        {
            KhachHang model = new KhachHang();
            HttpClient _httpClient = new HttpClient();
            string Id_khachang = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            HttpResponseMessage response1 = _httpClient.GetAsync("https://localhost:7266/api/Customer/" + Id_khachang).Result;

            if (response1.IsSuccessStatusCode)
            {
                string jsondata = response1.Content.ReadAsStringAsync().Result;
                model = JsonConvert.DeserializeObject<KhachHang>(jsondata);
            }
            model.MatKhau = khachHang.MatKhau;
            model.HoVaTen = khachHang.HoVaTen;
            StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpResponseMessage response2 = _httpClient.PutAsync("https://localhost:7266/api/Customer/", content).Result;
            if (response2.IsSuccessStatusCode)
            {
                return 1;
               
            }
            return 0;
        }
        public async Task<IActionResult> HuyOrder(string Id)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response =  httpClient.PutAsync("https://localhost:7266/api/ProfileCustomer/Huydonhang/" + Id, null).Result;
            if (response.IsSuccessStatusCode)
            {
                return Redirect("https://localhost:7047/Profile_Customer/order");
            }
            return Redirect("https://localhost:7047/Profile_Customer/OrderDetail" + Id);
        }
        public async Task<IActionResult> DetailVoucherKH( string Id)
        {
            HttpClient _httpClient = new HttpClient();
            Voucher voucher = new Voucher();
            HttpResponseMessage response = _httpClient.GetAsync("https://localhost:7266/api/VouCher/"+Id).Result;

            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                voucher = JsonConvert.DeserializeObject<Voucher>(jsondata);
              ViewBag.Voucher = voucher;
             
            }
            
            return View();
        }
    }
}
