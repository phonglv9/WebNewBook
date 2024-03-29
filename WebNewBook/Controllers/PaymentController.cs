﻿using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Common;
using WebNewBook.Model;
using WebNewBook.Models;
using WebNewBook.Models.GHNModels;
using WebNewBook.ViewModel;

namespace WebNewBook.Controllers
{
    public class PaymentController : Controller
    {
        public static Dictionary<string, string> vnp_TransactionStatus = new Dictionary<string, string>()
        {
            {"00","Giao dịch thành công" },
            {"01","Giao dịch chưa hoàn tất" },
            {"02","Giao dịch bị lỗi" },
            {"04","Giao dịch đảo (Khách hàng đã bị trừ tiền tại Ngân hàng nhưng GD chưa thành công ở VNPAY)" },
            {"05","VNPAY đang xử lý giao dịch này (GD hoàn tiền)" },
            {"06","VNPAY đã gửi yêu cầu hoàn tiền sang Ngân hàng (GD hoàn tiền)" },
            {"07","Giao dịch bị nghi ngờ gian lận" },
            {"09","GD Hoàn trả bị từ chối" }
        };
        
        private readonly HttpClient _httpClient;
        public PaymentController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
            _httpClient.DefaultRequestHeaders.Add("token", "71f04310-864d-11ed-b09a-9a2a48e971b0");
        }
        public async Task<KhachHang> GetKhachHang()
        {
            var email = User.Identity.Name;
            KhachHang khachHang = new KhachHang();
            HttpResponseMessage responseGet = await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Customer/{email}", null);
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                khachHang = JsonConvert.DeserializeObject<KhachHang>(jsonData);
            };
            return khachHang;
        }

        public List<CartItem> Giohangs()
        {


            List<CartItem> data = new List<CartItem>();
            if (string.IsNullOrEmpty(User.Identity.Name))
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
                List<ModelCart> gioHangs = new List<ModelCart>();
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "api/GioHang/GetLitsGH").Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonData1 = response.Content.ReadAsStringAsync().Result;
                    gioHangs = JsonConvert.DeserializeObject<List<ModelCart>>(jsonData1);
                    foreach (var a in gioHangs)
                    {
                        CartItem b = new CartItem();
                        if (User.Identity.Name == a.EmailKH)
                        {
                            b.DonGia = a.GiaBan;
                            b.Maasp = a.MaSanPham;
                            b.Soluong = a.SoLuong;
                            b.Tensp = a.TenSanPham;
                            b.hinhanh = a.HinhAnh;
                            b.ThanhTien = a.SoLuong * a.GiaBan;
                            data.Add(b);


                        }
                    }
                    return data;

                };


            }

            return data;

        }

        public class RemoveVC
        {
            public double? TotalShip { get; set; }
            public double? TotalOder { get; set; }
        }

        public JsonResult RemoveVoucher()
        {
           
            HttpContext.Session.Remove("idVoucher");
            HttpContext.Session.Remove("amoutVoucher");   

            RemoveVC removeVC = new RemoveVC();
            removeVC.TotalShip = Convert.ToDouble(HttpContext.Session.GetString("shiptotal"));
            removeVC.TotalOder = Convert.ToDouble(HttpContext.Session.GetString("amout"));
          
            return Json(removeVC, new System.Text.Json.JsonSerializerOptions());
        }
       

      
        //Lấy địa chỉ quận huyện
        public JsonResult GetListDistrict(int idProvin)
		{
          
            HttpResponseMessage responseDistrict =  _httpClient.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id="+idProvin).Result;

			District lstDistrict = new District();
           
			if (responseDistrict.IsSuccessStatusCode)
			{
				 string jsonData2 = responseDistrict.Content.ReadAsStringAsync().Result;

                lstDistrict = JsonConvert.DeserializeObject<District>(jsonData2);
            }
            return Json(lstDistrict, new System.Text.Json.JsonSerializerOptions());
		}
        //Lấy địa chỉ phường xã
        public JsonResult GetListWard(int idWard)//district không phải idWard
        {
            

            HttpResponseMessage responseWard = _httpClient.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=" + idWard).Result;

            Ward lstWard = new Ward();

            if (responseWard.IsSuccessStatusCode)
            {
                string jsonData2 = responseWard.Content.ReadAsStringAsync().Result;

                lstWard = JsonConvert.DeserializeObject<Ward>(jsonData2);
            }
            return Json(lstWard, new System.Text.Json.JsonSerializerOptions());
        }

		
		//Lấy phí ship ghn
		public JsonResult GetTotalShipping([FromBody]ShippingOrder shippingOrder)
        {
            _httpClient.DefaultRequestHeaders.Add("shop_id", "3630415");
            //StringContent contentshipping = new StringContent(JsonConvert.SerializeObject(shippingOrder), Encoding.UTF8, "application/json");
            HttpResponseMessage responseWShipping = _httpClient.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee?service_id="+ shippingOrder.service_id+"&insurance_value="+shippingOrder.insurance_value+"&coupon=&from_district_id="+shippingOrder.from_district_id+"&to_district_id="+shippingOrder.to_district_id+"&to_ward_code="+shippingOrder.to_ward_code+"&height="+shippingOrder.height+"&length="+shippingOrder.length+"&weight="+shippingOrder.weight+"&width="+shippingOrder.width+"").Result;

            Shipping shipping = new Shipping();
            if (responseWShipping.IsSuccessStatusCode)
            {
                string jsonData2 = responseWShipping.Content.ReadAsStringAsync().Result;

                shipping = JsonConvert.DeserializeObject<Shipping>(jsonData2);
                HttpContext.Session.SetString("shiptotal", shipping.data.total.ToString());
            }
            return Json(shipping, new System.Text.Json.JsonSerializerOptions());
        }
        public JsonResult GetTotalOder()
        {
            var total = Convert.ToDouble( HttpContext.Session.GetString("amout")) - Convert.ToDouble(HttpContext.Session.GetString("amoutVoucher"));

            return Json(total, new System.Text.Json.JsonSerializerOptions());
        }


        public async Task<IActionResult> CheckOut(string? messvnpay, string? idHoaDon)
        {
            var gioHangs = Giohangs();
            //Check validate nếu giỏ hàng trống không thể thanh toán
            if (gioHangs.Count() < 1)
            {
                return RedirectToAction("Index", "GioHang");
            }

            //Lấy địa chỉ tỉnh thành
            HttpResponseMessage responseProvin = _httpClient.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/province").Result;

            Provin lstprovin = new Provin();

            if (responseProvin.IsSuccessStatusCode)
            {
                string jsonData2 = responseProvin.Content.ReadAsStringAsync().Result;


                lstprovin = JsonConvert.DeserializeObject<Provin>(jsonData2);
                ViewBag.Provin = new SelectList(lstprovin.data, "ProvinceID", "ProvinceName");



            }
            var idVoucher = HttpContext.Session.GetString("idVoucher");
            double tongTien = Convert.ToDouble(HttpContext.Session.GetString("amout"));
            
                double menhGiaDK = Convert.ToDouble(HttpContext.Session.GetString("menhgiadk"));


            var khachHang = await GetKhachHang();
            //Khi khách hàng đã đăng nhập
            if (!string.IsNullOrEmpty(khachHang.ID_KhachHang))
            {
                ViewBag.KhachHang = khachHang;
                List<Voucher> lstVouchers = new List<Voucher>();
                List<VoucherCT> lstVoucherCTs = new List<VoucherCT>();
                List<VoucherPaymentVM> voucherPaymentVMs = new List<VoucherPaymentVM>();
                HttpResponseMessage responseVoucherCT = await _httpClient.GetAsync(_httpClient.BaseAddress + $"api/VoucherCT/VoucherKH/{khachHang.ID_KhachHang}");
                if (responseVoucherCT.IsSuccessStatusCode)
                {

                    string jsonData = responseVoucherCT.Content.ReadAsStringAsync().Result;
                    lstVoucherCTs = JsonConvert.DeserializeObject<List<VoucherCT>>(jsonData);
                    HttpResponseMessage responseVoucher = await _httpClient.GetAsync(_httpClient.BaseAddress + $"api/VouCher");
                    if (responseVoucherCT.IsSuccessStatusCode)
                    {
                        string jsonData2 = responseVoucher.Content.ReadAsStringAsync().Result;
                        lstVouchers = JsonConvert.DeserializeObject<List<Voucher>>(jsonData2);

                    }

                    foreach (var Vouchers in lstVouchers)
                    {
                        foreach (var VoucherCTs in lstVoucherCTs.Where(c => c.MaVoucher == Vouchers.Id))
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
                }


            }
            ViewBag.Cart = gioHangs;
            ViewBag.TongTien = tongTien;
            if (!string.IsNullOrEmpty(idHoaDon))
            {
                ViewBag.MessageVNPay = messvnpay;
                HoaDon hoaDon = new HoaDon();
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Payment/GetHoaDon/{idHoaDon}", null);
                if (response.IsSuccessStatusCode)
                {
                    string jsonData = response.Content.ReadAsStringAsync().Result;
                    hoaDon = JsonConvert.DeserializeObject<HoaDon>(jsonData);

                    return View("CheckOut", hoaDon);
                };
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Pay(HoaDon hoaDon, string payment,string adress_detail)
        {
           
            var tongTien = Convert.ToDouble(HttpContext.Session.GetString("amout"));
            double menhGiaVC = Convert.ToDouble(HttpContext.Session.GetString("amoutVoucher"));
            tongTien = tongTien - menhGiaVC;
            var tienShip = Convert.ToDouble(HttpContext.Session.GetString("shiptotal"));
            

            var idVoucher = HttpContext.Session.GetString("idVoucher");
            hoaDon.TongTien = tongTien;
            hoaDon.PhiGiaoHang = tienShip;
            hoaDon.MaGiamGia = idVoucher;
            hoaDon.DiaChiGiaoHang = hoaDon.DiaChiGiaoHang + adress_detail;

            if (hoaDon != null && !string.IsNullOrEmpty(payment) && hoaDon.TongTien != 0)
            {

                //Trang thái đơn hàng :

                //1.Đặt hàng
                //2.Đã thanh toán 
                //3.Đơn hàng thanh toán thất bại
                //4.Trả hàng
                //5.Thành công
                //6.Vô hiệu hóa

                var lstCart = Giohangs();
                ViewBag.SuccessMessage = "";
                KhachHang khachHang = new KhachHang();
                khachHang = await GetKhachHang();

                //Hóa đơn
                hoaDon.ID_HoaDon = "HD" + DateTime.Now.Ticks;
                if (khachHang.ID_KhachHang != null)
                {
                    hoaDon.MaKhachHang = khachHang.ID_KhachHang;
                }
                else
                {
                    //Nếu khách hàng chưa đăng nhập
                    hoaDon.MaKhachHang = "KHNOLOGIN";
                }

                hoaDon.NgayMua = DateTime.Now;
                if (payment == "1")
                {


                    hoaDon.TrangThai = 1;
                }
                else
                {

                    //Trạng thái chờ thanh toán:

                    hoaDon.TrangThai = 3;
                }
                StringContent contentHD = new StringContent(JsonConvert.SerializeObject(hoaDon), Encoding.UTF8, "application/json");
                HttpResponseMessage responseHD = await _httpClient.PostAsync("api/Payment/AddHoaDon", contentHD);
                if (!responseHD.IsSuccessStatusCode)
                {


                    ViewBag.SuccessMessage = "Đặt hàng thất bại vui lòng kiểm tra lại";
                    return View();
                }

                List<HoaDonCT> listHoaDonCTs = new List<HoaDonCT>();
                foreach (var item in lstCart)
                {
                    //Hóa đơn chi tiết

                    HoaDonCT hoaDonCT = new HoaDonCT();
                    hoaDonCT.ID_HDCT = "HDCT" + DateTime.Now.Ticks;
                    hoaDonCT.MaSanPham = item.Maasp;
                    hoaDonCT.MaHoaDon = hoaDon.ID_HoaDon;
                    hoaDonCT.SoLuong = item.Soluong;
                    hoaDonCT.GiaBan = item.DonGia;
                    listHoaDonCTs.Add(hoaDonCT);
                }
                StringContent contentHDCT = new StringContent(JsonConvert.SerializeObject(listHoaDonCTs), Encoding.UTF8, "application/json");
                HttpResponseMessage responseHDCT = await _httpClient.PostAsync("api/Payment/AddHoaDonCT", contentHDCT);

                if (responseHDCT.IsSuccessStatusCode)
                {
                    if (payment == "1")
                    {
                        StringContent contentHDCT2 = new StringContent(JsonConvert.SerializeObject(listHoaDonCTs), Encoding.UTF8, "application/json");
                        await _httpClient.PostAsync("api/Payment/UpdateSoLuongSP", contentHDCT2);

                        if (!string.IsNullOrEmpty(hoaDon.MaGiamGia))
                        {

                            await _httpClient.PutAsync(_httpClient.BaseAddress + $"api/VoucherCT/UpdateVoucherByPayment/{hoaDon.MaGiamGia}", null);

                        }
                        //if (khachHang.ID_KhachHang != "KHNOLOGIN")
                        //{

                        //    //Tích điểm
                        //    int fpoint = Convert.ToInt32(hoaDon.TongTien) / 100;
                        //    await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Fpoint/{hoaDon.ID_HoaDon}/{fpoint}/{khachHang.ID_KhachHang}", null);
                        //}

                        //Xóa giỏ hàng sau khi mua hàng
                        await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/GioHang/DeleteCarts/{khachHang.Email}", null);

                        //Gửi mail đơn hàng 
                        await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Payment/SendMailOder/{hoaDon.ID_HoaDon}", null);
                        HttpContext.Session.Clear();
                        Response.Cookies.Delete("Cart");
                        ViewBag.SuccessMessage = "Đặt hàng thành công";
                        return View();
                    }

                }
                else
                {
                    ViewBag.SuccessMessage = "Đặt hàng thất bại";
                }

                if (payment == "2")
                {
                    if (string.IsNullOrEmpty(VNPayConfig.vnp_TmnCode) || string.IsNullOrEmpty(VNPayConfig.vnp_HashSecret))
                    {
                        ViewBag.SuccessMessage = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret";
                        return View();
                    }

                    //Set id mã giảm giá gửi sang sau khi thanh toán thành công
                    if (hoaDon.MaGiamGia != null)
                    {
                        HttpContext.Session.SetString("idVoucherVNPAY", hoaDon.MaGiamGia.ToString());
                    }


                    //Hóa đơn VNpay
                    OrderInfo order = new OrderInfo();
                    order.OrderId = hoaDon.ID_HoaDon; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
                    order.Amount = tongTien + tienShip; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
                    order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"                   
                    order.CreatedDate = hoaDon.NgayMua;

                    VnPayLibrary vnpay = new VnPayLibrary();
                    vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                    vnpay.AddRequestData("vnp_Command", "pay");
                    vnpay.AddRequestData("vnp_TmnCode", VNPayConfig.vnp_TmnCode);
                    vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());//Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
                                                                                        //if (!string.IsNullOrEmpty(request.NganHang))
                    vnpay.AddRequestData("vnp_BankCode", "NCB" /*request.NganHang*/);
                    vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
                    vnpay.AddRequestData("vnp_CurrCode", "VND");
                    vnpay.AddRequestData("vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString());
                    vnpay.AddRequestData("vnp_Locale", "vn");
                    vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);

                    vnpay.AddRequestData("vnp_ReturnUrl", VNPayConfig.vnp_Returnurl);
                    vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());// Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
                                                                                 //Add Params of 2.1.0 Version
                                                                                 //vnpay.AddRequestData("vnp_ExpireDate", request.ThoiHanThanhToan);
                                                                                 //Billing
                    vnpay.AddRequestData("vnp_Bill_Mobile", hoaDon.SDT.Trim());
                    vnpay.AddRequestData("vnp_Bill_Email", hoaDon.Email.Trim());
                    var fullName = hoaDon.TenNguoiNhan.Trim();
                    if (!String.IsNullOrEmpty(fullName))
                    {
                        var indexof = fullName.IndexOf(' ');
                        vnpay.AddRequestData("vnp_Bill_FirstName", fullName.Substring(0, indexof));
                        vnpay.AddRequestData("vnp_Bill_LastName", fullName.Substring(indexof + 1, fullName.Length - indexof - 1));
                    }

                    string paymentUrl = vnpay.CreateRequestUrl(VNPayConfig.vnp_Url, VNPayConfig.vnp_HashSecret);


                    if (khachHang.ID_KhachHang != "KHNOLOGIN")
                    {
                        //set mã khách hàng gửi sang cổng thanh toán 
                        if (khachHang.Email != null)
                        {
                            HttpContext.Session.SetString("emailCustomer", khachHang.Email.ToString());
                            HttpContext.Session.SetString("idCustomer", khachHang.ID_KhachHang.ToString());
                        }

                    }
                    return Redirect(paymentUrl);
                }
            }
            return View();

        }

        public async Task<IActionResult> VNPayReturn([FromQuery] VNPayReturn request)
        {
            var idVoucherVNPAY = HttpContext.Session.GetString("idVoucherVNPAY");
            var emailCustomer = HttpContext.Session.GetString("emailCustomer");
            var idCustomer = HttpContext.Session.GetString("idCustomer");
            var Infoid = request.vnp_OrderInfo;
            var getIdHoaDon = (from t in Infoid
                               where char.IsDigit(t)
                               select t).ToArray();
            var idHoaDon = "HD" + new string(getIdHoaDon);

            request.message = "Không xác định được trạng thái";
            if (vnp_TransactionStatus.ContainsKey(request.vnp_TransactionStatus))
            {
                request.message = vnp_TransactionStatus[request.vnp_TransactionStatus];
            }
            ViewBag.vnp_TransactionStatus = request.vnp_TransactionStatus;
            if (request.vnp_TransactionStatus != "00")
            {

                ViewBag.Message = request.message;
                return RedirectToAction("CheckOut", "Payment", new { messvnpay = request.message, idHoaDon = idHoaDon });
            }

            if (!string.IsNullOrEmpty(idHoaDon))
            {
                await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Payment/UpdateTrangThai/{idHoaDon}", null);
                await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Payment/UpdateSLVNPay/{idHoaDon}", null);


            }

            //Tích điểm
            //if (idCustomer != "KHNOLOGIN")
            //{
            //    int fpoint = Convert.ToInt32(request.vnp_Amount) / 10000;
            //    await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Fpoint/{idHoaDon}/{fpoint}/{idCustomer}", null);
            //}
            //Thay đổi trạng thái voucher
            if (!string.IsNullOrEmpty(idVoucherVNPAY))
            {

                await _httpClient.PutAsync(_httpClient.BaseAddress + $"api/VoucherCT/UpdateVoucherByPayment/{idVoucherVNPAY}", null);

            }
            await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/GioHang/DeleteCarts/{emailCustomer}", null);
            //Gửi mail đơn hàng 
            await _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Payment/SendMailOder/{idHoaDon}", null);
            HttpContext.Session.Clear();
            Response.Cookies.Delete("Cart");
            ViewBag.Message = request.message;
            return View();
        }
        //[HttpPost]
        public class VoucherAjax { 
        
            public string? Mess { get; set; }
            public double? MenhGiaVoucher { get; set; }
            public string? MaVoucher { get; set; }
            public double? TotalOder { get; set; }
            public double? TotalShip { get; set; }
        
        }

        public async Task<JsonResult> ApDungVouCher(string maVoucher)
        {
            var tongTien = Giohangs().Sum(c => c.ThanhTien);
            var ngayHienTai = DateTime.Now;
            KhachHang khachHang = new KhachHang();
            khachHang = await GetKhachHang();
            VoucherAjax voucherAjax = new VoucherAjax();
            if (!string.IsNullOrEmpty(khachHang.ID_KhachHang))
            {
                if (!string.IsNullOrEmpty(maVoucher))
                {
                    VoucherCT voucherCT = new VoucherCT();
                    HttpResponseMessage responseVoucherCT = await _httpClient.GetAsync(_httpClient.BaseAddress + $"api/VoucherCT/{maVoucher}");

                    string jsonData = responseVoucherCT.Content.ReadAsStringAsync().Result;
                    voucherCT = JsonConvert.DeserializeObject<VoucherCT>(jsonData);
                    if (voucherCT != null)
                    {
                        if (voucherCT.MaKhachHang == khachHang.ID_KhachHang)
                        {
                            Voucher voucher = new Voucher();
                            HttpResponseMessage responseVoucher = await _httpClient.GetAsync(_httpClient.BaseAddress + $"api/VouCher/{voucherCT.MaVoucher}");
                            string jsonData2 = responseVoucher.Content.ReadAsStringAsync().Result;
                            voucher = JsonConvert.DeserializeObject<Voucher>(jsonData2);
                            if (tongTien >= voucher.MenhGiaDieuKien && ngayHienTai >= voucherCT.NgayBatDau && ngayHienTai <= voucherCT.NgayHetHan && voucherCT.TrangThai == 1)
                            {

                                HttpContext.Session.SetString("idVoucher", maVoucher.ToString());
                                HttpContext.Session.SetString("amoutVoucher", voucher.MenhGia.ToString());
                                HttpContext.Session.SetString("menhgiadk", voucher.MenhGiaDieuKien.ToString());     
                                

                                tongTien = tongTien - voucher.MenhGia;
                                voucherAjax.Mess = "OK";
                                voucherAjax.MenhGiaVoucher = voucher.MenhGia;
                                voucherAjax.MaVoucher = voucherCT.Id;
                                voucherAjax.TotalOder = tongTien;
                                voucherAjax.TotalShip = Convert.ToDouble(HttpContext.Session.GetString("shiptotal"));
                              


                            }
                            else if (voucherCT.TrangThai == 2)
                            {
                                voucherAjax.Mess = "Voucher đã hết hiệu lực";
                            }
                            else if (ngayHienTai < voucherCT.NgayBatDau)
                            {
                                voucherAjax.Mess = "Voucher chưa phát hàng, bạn có thể sử dụng vào lúc" + voucherCT.NgayBatDau;
                            }
                            else if (ngayHienTai > voucherCT.NgayHetHan)
                            {
                                voucherAjax.Mess = "Voucher đã hết thời hạn sử dụng";
                            }
                            else if (tongTien < voucher.MenhGiaDieuKien)
                            {
                                var dkVoucher = voucher.MenhGiaDieuKien - tongTien;
                                voucherAjax.Mess = "Số tiền bạn mua không đủ điều kiện để dùng, bạn cần mua thêm " + dkVoucher + "đ";
                            }
                            else
                            {
                                voucherAjax.Mess = "Mã giảm giá không hợp lệ";
                            }
                        }



                    }
                    else
                    {
                        voucherAjax.Mess = "Mã giảm giá không hợp lệ";
                    }





                }
                else
                {
                    voucherAjax.Mess = "Vui lòng nhập mã giảm giá";
                }
            }
            return Json(voucherAjax, new System.Text.Json.JsonSerializerOptions());
        }
    }
}
