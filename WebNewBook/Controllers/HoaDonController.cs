using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using WebNewBook.Model;
using WebNewBook.Models;
using X.PagedList;
using WebNewBook.API.ModelsAPI;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.Text.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Xml.Linq;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HoaDonController : Controller
    {
        Uri link = new Uri("https://localhost:7266/api");
        HttpClient client;
        public HoaDonController()
        {
            client = new HttpClient();
            client.BaseAddress = link;
          
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }
        public async Task<IActionResult> Index(string? timkiem, int? page, string mess, int trangThai = 1)
        {
             
            ViewBag.TitleAdmin = "Hóa Đơn";
            ViewBag.TimKiem = timkiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            var pageNumber = page ?? 1;
            List<ViewHoaDon> lissttl = new List<ViewHoaDon>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/HoaDon/GetHD").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttl = JsonConvert.DeserializeObject<List<ViewHoaDon>>(data);
               
            }
            if (!string.IsNullOrEmpty(timkiem))
            {
                timkiem = timkiem.ToLower();
                lissttl = lissttl.Where(a => a.hoaDon.ID_HoaDon.ToLower().Contains(timkiem)).ToList();
                
            }    
            if (trangThai != null)
            {
                switch (trangThai)
                {
                    
                    case 1:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 1 || c.hoaDon.TrangThai==2).ToList();
                        break;
                    case 2:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 2).ToList();
                        break;
            
                    case 4:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 4).ToList();
                        break;
                    case 5:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 5).ToList();
                        break;
                    case 6:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 6).ToList();
                        break;
                    case 7:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 7).ToList();
                        break;
                    case 8:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 8).ToList();
                        break;
                    case 9:
                        lissttl = lissttl.Where(c => c.hoaDon.TrangThai == 9).ToList();
                        break;
                    default:
                        lissttl = lissttl.ToList();
                        break;
                }
            }
            List<ViewHoaDon> lissttl2 = new List<ViewHoaDon>();
            foreach(var item in lissttl.OrderByDescending(c=>c.hoaDon.NgayMua))
            {
                if (item.hoaDon.TrangThai != 3)
                {
                    lissttl2.Add(item);
                }
            }

            ViewBag.DataHD = lissttl2.ToPagedList(pageNumber, 20);
               
                //ViewBag.dataNew = lissttl.ToPagedList((int)page, (int)pagesize);
                return View("IndexHD");
            

        }
        public async Task<IActionResult> ChiTiet(string id)
        {
            ViewBag.TitleAdmin = "Chi tiết hóa đơn";
            List<ViewHoaDonCT> lissttlhdct = new List<ViewHoaDonCT>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/HoaDon/getlistid/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                lissttlhdct = JsonConvert.DeserializeObject<List<ViewHoaDonCT>>(data);

                //Toi uu cau lenh 
                //var hoadon = lissttlhdct.Where(c=>c.hoaDon.ID_HoaDon == id).FirstOrDefault().hoaDon;
                
                //Thông tin khách hàng
                ViewBag.IDLogin = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.ID_KhachHang).FirstOrDefault();
                ViewBag.NameLogin = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.HoVaTen).FirstOrDefault();
                ViewBag.SDTLogin = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.SDT).FirstOrDefault();
                ViewBag.EmailLogin = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.KhachHang.Email).FirstOrDefault();

                var hoaDon = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).FirstOrDefault();
                Voucher voucher = new Voucher();
                if (hoaDon !=null)
                {
                    HttpResponseMessage responsevc = client.GetAsync(client.BaseAddress + $"/HoaDon/GetPriceVoucher/{hoaDon.hoaDon.MaGiamGia}").Result;
                    if (responsevc.IsSuccessStatusCode)
                    {
                        string data2 = responsevc.Content.ReadAsStringAsync().Result;
                        voucher = JsonConvert.DeserializeObject<Voucher>(data2);
                        ViewBag.PriceVoucher = voucher.MenhGia;
                    }

                }

                //Thông tin hóa đơn
                ViewBag.IdHoaDon = id;
                ViewBag.Namekh = lissttlhdct.Where(c=>c.hoaDon.ID_HoaDon==id).Select(c=>c.hoaDon.TenNguoiNhan).FirstOrDefault();
                ViewBag.sdtkh = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.SDT).FirstOrDefault();
                ViewBag.ghichu = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.GhiChu).FirstOrDefault();
                ViewBag.diachi = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.DiaChiGiaoHang).FirstOrDefault();
                ViewBag.ngaymua = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.NgayMua).FirstOrDefault();
                ViewBag.tongtien= lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.TongTien).FirstOrDefault(); 
                ViewBag.trangthai= lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.TrangThai).FirstOrDefault();
                ViewBag.lydohuyudon= lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.Lydohuy).FirstOrDefault();
                ViewBag.phigiaohang= lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).Select(c => c.hoaDon.PhiGiaoHang).FirstOrDefault();

                var lstProduct  = new List<SanPham>();
                HttpResponseMessage respProduct = client.GetAsync("https://localhost:7266/SanPham").Result;
                if (respProduct.IsSuccessStatusCode)
                {
                    string dataProduct = respProduct.Content.ReadAsStringAsync().Result;
                    lstProduct = JsonConvert.DeserializeObject<List<SanPham>>(dataProduct);
                    ViewBag.ListProducts = lstProduct;


                }
            }



            ViewBag.HDCT = lissttlhdct.GroupBy(a => a.sanPham.TenSanPham);
            return View("IndexHDCT");
        }
        // sửa trạng thái đơn hàng
        public async Task<IActionResult> Sua(string id, int name,string? lyDoHuy) 
        {
            Console.WriteLine(lyDoHuy);
           
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + $"/HoaDon/UpdateTT/{id}/{name}?lydohuy={lyDoHuy}").Result;
            if (name==5)
            {
                HttpResponseMessage response1 = client.PostAsync(client.BaseAddress + $"/Fpoint/{id}", null).Result;
            }


            return Redirect("Index");
        }


        // update thông tin người nhận hàng
        public async Task<IActionResult> UpdateThongtinnguoinhan(ViewHoaDon viewHoaDon)
        {

            StringContent content = new StringContent(JsonConvert.SerializeObject(viewHoaDon.hoaDon), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/Hoadon/UpdateRecipientProfile", content).Result;
            if (response.IsSuccessStatusCode)
            {
         
                Console.WriteLine($"Status: {response.StatusCode}; Msg: {await response.Content.ReadAsStringAsync()}");
            }
            else
            {
                Console.WriteLine($"Status: {response.StatusCode}; Msg: {await response.Content.ReadAsStringAsync()}");
            }
            return Redirect("ChiTiet/"+viewHoaDon.hoaDon.ID_HoaDon);
        }
        [HttpPost]
        public int AddProduct(string ListIdProduct, string IDHoaDon)
        {
            if (ListIdProduct !=null)
            {
             
                string[] arr = ListIdProduct.Split(',');
               
                foreach (var x in arr)
                {
                    Console.WriteLine($"Lượt 1:Idhoasdon {IDHoaDon} và id sp:{x}");
                    HttpResponseMessage response = client.PostAsync(client.BaseAddress + $"/HoaDonCT/AddHoaDonCt?mahd={IDHoaDon}&masp={x}",null).Result;
                    Console.WriteLine($"Status: {response.StatusCode}; Msg: { response.Content.ReadAsStringAsync()}");
                } 
            }
          
            return 1;
        }

        public async   Task<IActionResult> UpdateQuantityProduct(string mahdct, string soluong , string mahd)
        {
            HttpResponseMessage response = client.PutAsync(client.BaseAddress + $"/HoaDonCT/UpdateHoaDonCT?mahdct={mahdct}&soluong={soluong}",null).Result;
            return Redirect($"ChiTiet/{mahd}");
        }

        public async Task<IActionResult> DeleteProductinOrder(string mahdct,string mahd)
        {
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + $"/HoaDonCT/DeleteHoaDonCT/{mahdct}").Result;
            return Redirect($"ChiTiet/{mahd}");
        }

        public async Task<IActionResult> addOrderAdmin(ViewHoaDon viewHoaDon)
        {
            viewHoaDon.hoaDon.ID_HoaDon= Guid.NewGuid().ToString();
            viewHoaDon.hoaDon.MaKhachHang = "KHNOLOGIN";
            viewHoaDon.hoaDon.NgayMua = DateTime.Now;
            viewHoaDon.hoaDon.TongTien = 0;
            viewHoaDon.hoaDon.TrangThai = 1;
            //phóng sẽ sửa code
            viewHoaDon.hoaDon.WardID = "NO";
            viewHoaDon.hoaDon.ProvinID = "NO";
            viewHoaDon.hoaDon.DistrictID = "NO";
            StringContent content = new StringContent(JsonConvert.SerializeObject(viewHoaDon.hoaDon), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/Hoadon/AddOrderAdmin", content).Result;
            if (response.IsSuccessStatusCode)
            {

                Console.WriteLine($"Status: {response.StatusCode}; Msg: {await response.Content.ReadAsStringAsync()}");
            }
            else
            {
                Console.WriteLine($"Status: {response.StatusCode}; Msg: {await response.Content.ReadAsStringAsync()}");
            }
            return Redirect("Index");
        }

    }
}
