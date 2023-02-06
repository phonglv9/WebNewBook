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
using Microsoft.AspNetCore.Mvc.Rendering;
using WebNewBook.Models.GHNModels;

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
            client.DefaultRequestHeaders.Add("token", "71f04310-864d-11ed-b09a-9a2a48e971b0");

        }
        #region GHN 
        //Lấy địa chỉ quận huyện
        public JsonResult GetListDistrict(int idProvin)
        {

            HttpResponseMessage responseDistrict = client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=" + idProvin).Result;

            District lstDistrict = new District();

            if (responseDistrict.IsSuccessStatusCode)
            {
                string jsonData2 = responseDistrict.Content.ReadAsStringAsync().Result;

                lstDistrict = JsonConvert.DeserializeObject<District>(jsonData2);
            }
            return Json(lstDistrict, new System.Text.Json.JsonSerializerOptions());
        }
        //Lấy địa chỉ phường xã
        public JsonResult GetListWard(int idWard)
        {


            HttpResponseMessage responseWard = client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=" + idWard).Result;

            Ward lstWard = new Ward();

            if (responseWard.IsSuccessStatusCode)
            {
                string jsonData2 = responseWard.Content.ReadAsStringAsync().Result;

                lstWard = JsonConvert.DeserializeObject<Ward>(jsonData2);
            }
            return Json(lstWard, new System.Text.Json.JsonSerializerOptions());
        }


        //Lấy phí ship ghn
        public JsonResult GetTotalShipping([FromBody] ShippingOrder shippingOrder)
        {
            client.DefaultRequestHeaders.Add("shop_id", "3630415");
            //StringContent contentshipping = new StringContent(JsonConvert.SerializeObject(shippingOrder), Encoding.UTF8, "application/json");
            HttpResponseMessage responseWShipping = client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee?service_id=" + shippingOrder.service_id + "&insurance_value=" + shippingOrder.insurance_value + "&coupon=&from_district_id=" + shippingOrder.from_district_id + "&to_district_id=" + shippingOrder.to_district_id + "&to_ward_code=" + shippingOrder.to_ward_code + "&height=" + shippingOrder.height + "&length=" + shippingOrder.length + "&weight=" + shippingOrder.weight + "&width=" + shippingOrder.width + "").Result;

            Shipping shipping = new Shipping();
            if (responseWShipping.IsSuccessStatusCode)
            {
                string jsonData2 = responseWShipping.Content.ReadAsStringAsync().Result;

                shipping = JsonConvert.DeserializeObject<Shipping>(jsonData2);
                HttpContext.Session.SetString("shiptotaladmin", shipping.data.total.ToString());
            }
            return Json(shipping, new System.Text.Json.JsonSerializerOptions());
        }
        
        #endregion
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
                var hoadonCT = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).FirstOrDefault().hoaDon;
                var thongtinkh = lissttlhdct.Where(c => c.hoaDon.ID_HoaDon == id).FirstOrDefault().KhachHang;

                //Thông tin khách hàng
                ViewBag.IDLogin = thongtinkh.ID_KhachHang;
                ViewBag.NameLogin = thongtinkh.HoVaTen;
                ViewBag.SDTLogin = thongtinkh.SDT;
                ViewBag.EmailLogin = thongtinkh.Email;

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
                ViewBag.Namekh = hoadonCT.TenNguoiNhan;
                ViewBag.sdtkh = hoadonCT.SDT;
                ViewBag.ghichu = hoadonCT.GhiChu;
                ViewBag.diachi = hoadonCT.DiaChiGiaoHang;
                ViewBag.ngaymua = hoadonCT.NgayMua;
                ViewBag.tongtien= hoadonCT.TongTien; 
                ViewBag.trangthai= hoadonCT.TrangThai;
                ViewBag.lydohuyudon= hoadonCT.Lydohuy;
                ViewBag.phigiaohang= hoadonCT.PhiGiaoHang;
               
                
                //Địa chỉ ghn

                //Lấy địa chỉ tỉnh thành
                HttpResponseMessage responseProvin = client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/province").Result;

                Provin lstprovin = new Provin();

                if (responseProvin.IsSuccessStatusCode)
                {
                    string jsonData2 = responseProvin.Content.ReadAsStringAsync().Result;


                    lstprovin = JsonConvert.DeserializeObject<Provin>(jsonData2);
                    ViewBag.Provin = new SelectList(lstprovin.data, "ProvinceID", "ProvinceName", hoadonCT.ProvinID);



                }
                //Lấy địa chỉ quận / huyện
                HttpResponseMessage responseDistrict = client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=" + hoadonCT.ProvinID).Result;

                District lstDistrict = new District();

                if (responseDistrict.IsSuccessStatusCode)
                {
                    string jsonDataDistrict = responseDistrict.Content.ReadAsStringAsync().Result;
                    lstDistrict = JsonConvert.DeserializeObject<District>(jsonDataDistrict);
                    ViewBag.District = new SelectList(lstDistrict.data, "DistrictID", "DistrictName", hoadonCT.DistrictID);
                }

                //Lấy phường xã
                HttpResponseMessage responseWard = client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=" + hoadonCT.DistrictID).Result;

                Ward lstWard = new Ward();

                if (responseWard.IsSuccessStatusCode)
                {
                    string jsonData2 = responseWard.Content.ReadAsStringAsync().Result;

                    lstWard = JsonConvert.DeserializeObject<Ward>(jsonData2);
                    ViewBag.Ward = new SelectList(lstWard.data, "WardCode", "WardName", hoadonCT.WardID);
                }


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
        public async Task<IActionResult> UpdateThongtinnguoinhan(ViewHoaDon viewHoaDon, string adress_detail)
        {
            viewHoaDon.hoaDon.DiaChiGiaoHang = viewHoaDon.hoaDon.DiaChiGiaoHang + adress_detail;
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
