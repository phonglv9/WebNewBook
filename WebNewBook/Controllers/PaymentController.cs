using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebNewBook.Common;
using WebNewBook.Model;
using WebNewBook.Models;

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
        public IActionResult CheckOut()
        {

            ViewBag.Cart = Giohangs;
            ViewBag.TongTien = Giohangs.Sum(c => c.ThanhTien);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Pay(HoaDon hoaDon, string payment)
        {
            if (hoaDon != null && !string.IsNullOrEmpty(payment))
            {

                //Trang thái đơn hàng :
                //0.Không tồn tại
                //1.Đặt hàng
                //2.Đã thanh toán 
                //3.Đơn hàng thanh toán thất bại
                //4.Trả hàng
                //5.Thành công         

                var lstCart = Giohangs;
                ViewBag.SuccessMessage = "";
                List<KhachHang>? ListKhachHangs = new List<KhachHang>();
                ListKhachHangs = await GetKhachHang();
                var KhachHang = ListKhachHangs.Where(c => c.Email == User.Identity.Name).FirstOrDefault();
                if (KhachHang == null)

                {
                    //Tạo mới khách hàng khi chưa login
                    KhachHang = new KhachHang();
                    KhachHang.ID_KhachHang = "KHX" + DateTime.Now.Ticks;
                    KhachHang.HoVaTen = hoaDon.TenNguoiNhan;
                    KhachHang.DiaChi = hoaDon.DiaChiGiaoHang;
                    KhachHang.DiemTichLuy = 1;
                    StringContent contentKH = new StringContent(JsonConvert.SerializeObject(KhachHang), Encoding.UTF8, "application/json");
                    HttpResponseMessage responseKH = await _httpClient.PostAsync("api/Customer", contentKH);
                    if (!responseKH.IsSuccessStatusCode)
                    {
                        ViewBag.SuccessMessage = "Đặt hàng thất bại vui lòng kiểm tra lại";
                        return View();
                    }

                }

                //Hóa đơn
                hoaDon.ID_HoaDon = "HD" + DateTime.Now.Ticks;
                hoaDon.MaKhachHang = KhachHang.ID_KhachHang;
                hoaDon.NgayMua = DateTime.Now;
                if (payment == "1")
                {
                    hoaDon.TrangThai = 1;
                }
                else
                {

                    //Thanh toán:

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

                    //Hóa đơn VNpay
                    OrderInfo order = new OrderInfo();
                    order.OrderId = hoaDon.ID_HoaDon; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
                    order.Amount = hoaDon.TongTien; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
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
                    return Redirect(paymentUrl);
                }
            }
            return View();

        }

        public IActionResult VNPayReturn([FromQuery] VNPayReturn request)
        {
            request.message = "Không xác định được trạng thái";
            if (vnp_TransactionStatus.ContainsKey(request.vnp_TransactionStatus))
            {
                request.message = vnp_TransactionStatus[request.vnp_TransactionStatus];
            }
            ViewBag.vnp_TransactionStatus = request.vnp_TransactionStatus;
            if (request.vnp_TransactionStatus != "00")
            {

                ViewBag.Message = request.message;
                return View();



            }
            var Infoid = request.vnp_OrderInfo;
            var getIdHoaDon = (from t in Infoid
                               where char.IsDigit(t)
                               select t).ToArray();
            var idHoaDon = new string(getIdHoaDon);
            if (!string.IsNullOrEmpty(idHoaDon))
            {
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + $"api/Payment/UpdateTrangThai/{"HD" + idHoaDon}", null).Result;

            }

            Response.Cookies.Delete("Cart");
            ViewBag.Message = request.message;
            return View();
        }
    }
}
