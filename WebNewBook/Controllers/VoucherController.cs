using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.ViewModel;
using X.PagedList;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VoucherController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;

        public VoucherController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }

        public async Task<IActionResult> Index(int? page, string search, DateTime? startdate, DateTime? enddate, int? type)
        {
            ViewBag.TitleAdmin = "Voucher";

            ViewBag.Search = search;
            ViewBag.Startdate = startdate;
            ViewBag.Enddate = enddate;
            ViewBag.Type = type;

            var pageNumber = page ?? 1;
            List<Voucher> Getvoucher = new List<Voucher>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Voucher").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                Getvoucher = JsonConvert.DeserializeObject<List<Voucher>>(jsondata);
            }
            if (enddate != null)
            {
                enddate = enddate.Value.AddDays(1);
            }


            ViewBag.lstvoucher = Getvoucher.Where(c => c.TrangThai != 0 && ((!string.IsNullOrEmpty(search) ? c.Id.StartsWith(search) : true) || (!string.IsNullOrEmpty(search) ? c.TenPhatHanh.Contains(search) : true))
                                                                    && (startdate.HasValue ? c.StartDate >= startdate.Value : true)
                                                                    && (enddate.HasValue ? c.EndDate <= enddate.Value : true)
                                                                    && (type != null ? c.HinhThuc == type : true)).OrderByDescending(c => c.Createdate).ToPagedList(pageNumber, 10);
          
            return View();
        }


        /// Tạo đợt phát hành 
        public async Task<int> Add(Voucher voucher)
        {

            voucher.MaNhanVien = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            voucher.Id = "1";
            HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "/Voucher", voucher).Result;
            if (response.IsSuccessStatusCode)
            {
                return 1;
            }
            return 0;

        }

        public async Task<IActionResult> Detail(string Id, VoucherModel voucherModel)
        {
            HttpResponseMessage response_1 = _httpClient.GetAsync(_httpClient.BaseAddress + "/Voucher/" + Id).Result;

            if (response_1.IsSuccessStatusCode)
            {
                string jsondata_1 = response_1.Content.ReadAsStringAsync().Result;

                voucherModel.Voucher = JsonConvert.DeserializeObject<Voucher>(jsondata_1);
                ViewBag.Hinhthuc = voucherModel.Voucher.HinhThuc;
                Console.WriteLine(voucherModel.Voucher.HinhThuc);

            }
            HttpResponseMessage response_2 = _httpClient.GetAsync(_httpClient.BaseAddress + "/VoucherCT/CallIdPH/" + voucherModel.Voucher.Id).Result;

            if (response_2.IsSuccessStatusCode)
            {
                string jsondata_2 = response_2.Content.ReadAsStringAsync().Result;
                List<VoucherCT> voucherCTs = new List<VoucherCT>();
                voucherCTs = JsonConvert.DeserializeObject<List<VoucherCT>>(jsondata_2);
                ViewBag.lstvoucherCT0 = voucherCTs.Where(c => c.TrangThai == 0).OrderByDescending(c => c.CreateDate).ToList();
                ViewBag.lstvoucherCT1 = voucherCTs.OrderByDescending(c => c.CreateDate).ToList();
                ViewBag.lstvoucherCT2 = voucherCTs.Where(c => c.TrangThai == 2).OrderByDescending(c => c.CreateDate).ToList();
               

            }
            HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer").Result;
            List<KhachHang> lstCustomer = new List<KhachHang>();
            if (responseMessage.IsSuccessStatusCode)
            {
                string jsondata_lstkhachhang = responseMessage.Content.ReadAsStringAsync().Result;
                lstCustomer = JsonConvert.DeserializeObject<List<KhachHang>>(jsondata_lstkhachhang);
                ViewBag.SoluongKhachHang = lstCustomer.Count();


            }
            var listItem = new List<SelectListItem>();
            lstCustomer.ForEach(s =>
            {
                listItem.Add(new SelectListItem { Text = s.HoVaTen + " - " + s.SDT, Value = s.ID_KhachHang });
            });
            ViewBag.lstItemKhachhang = listItem;
            return View(voucherModel);
        }


        public async Task<IActionResult> Delete(string Id)
        {


            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Voucher/" + Id, null).Result;
            return RedirectToAction("Index");
        }




        public async Task<int> CreateListvoucher(int quantityVoucher, int sizeVoucher, string startTextVoucher, string endTextVoucher, string maVoucher)
        {

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/VoucherCT/AddAutomatically?quantityVoucher=" + quantityVoucher + "&sizeVoucher=" + sizeVoucher + "&startTextVoucher=" + startTextVoucher + "&endTextVoucher=" + endTextVoucher + "&maVoucher=" + maVoucher, null).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                return 1;

            }
            return 0;

        }


        public static string UpLoadFile(Microsoft.AspNetCore.Http.IFormFile file, string newname)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "File");
                CreateIfMissing(path);
                string pathfile = Path.Combine(Directory.GetCurrentDirectory(), "File", newname);
                string pathfile_Db = Path.Combine("File", newname);
                var supportedtypes = new[] { "xlsx", "xls" };
                var fileext = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedtypes.Contains(fileext.ToLower()))
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathfile, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return pathfile;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return "lỗi";
            }
        }


        public static void CreateIfMissing(string path)
        {
            try
            {
                bool a = Directory.Exists(path);
                if (!a)
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();

            }
        }
        public async Task<IActionResult> CreateImPortExcel(IFormFile file, string maPhathanh)
        {

            try
            {
                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string image = file.FileName;
                    string path = UpLoadFile(file, image);

                    using var httpClient = new HttpClient()
                    {
                        Timeout = TimeSpan.FromDays(1)
                    };
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "dotnet-http-client");
                    using var content = new MultipartFormDataContent();
                    using var stream = System.IO.File.OpenRead(path);
                    content.Add(new StreamContent(stream), "file", Path.GetFileName(path));
                    var url = "https://localhost:7266/api/VoucherCT/AddImportExcer?Phathanh=" + maPhathanh;
                    using var response = await httpClient.PostAsync(url, content);
                    Console.WriteLine($"Status: {response.StatusCode}; Msg: {await response.Content.ReadAsStringAsync()}");

                }

                return Redirect("https://localhost:7047/Voucher/Detail/" + maPhathanh);
            }
            catch (Exception e)
            {

                return View(e.Message);
            }
        }
        // tạo chậm
        public async Task<int> Create(VoucherCT voucherCT)
        {


            try
            {
                List<VoucherCT> GetvoucherCT = new List<VoucherCT>();
                HttpResponseMessage response_voucher = _httpClient.GetAsync(_httpClient.BaseAddress + "/VoucherCT/Addvoucher").Result;
                if (response_voucher.IsSuccessStatusCode)
                {
                    string jsondata = response_voucher.Content.ReadAsStringAsync().Result;
                    GetvoucherCT = JsonConvert.DeserializeObject<List<VoucherCT>>(jsondata);
                    foreach (var x in GetvoucherCT)
                    {
                        if (x.Id == voucherCT.Id)
                        {
                            return 2;
                        }
                    }
                }

                StringContent content = new StringContent(JsonConvert.SerializeObject(voucherCT), Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/VoucherCT/AddManually", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    voucherCT = JsonConvert.DeserializeObject<VoucherCT>(apiResponse);
                    return 1;
                }
                Console.WriteLine($"Status: {response.StatusCode}; Msg: {await response.Content.ReadAsStringAsync()}");
                return 0;




            }
            catch (Exception e)
            {

                return 0;
            }


        }
        public async Task<IActionResult> Update(Voucher voucher)
        {
            if (voucher.DiemDoi < 0 || voucher.DiemDoi > 99900)
            {
                ViewBag.checkdiemdoi = "Điểm đổi phải từ 1 đến 99,999";
                return RedirectToAction("Index");
            }
            if (voucher.HinhThuc==2)
            {
               
                voucher.DiemDoi = null;
            }
            StringContent content = new StringContent(JsonConvert.SerializeObject(voucher), Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Voucher", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                voucher = JsonConvert.DeserializeObject<Voucher>(apiResponse);
            }
            return RedirectToAction("Index");
        }

        public IActionResult dowloadFilemau()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("File");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Mã Voucher";

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "file_mau" + ".xlsx");
        }


        public List<string> Get_Id(string getId)
        {
            List<string> lstId = new List<string>();
            string[] arr = getId.Split(',');
            foreach (var x in arr)
            {
                lstId.Add(x);
            }
            return lstId;

        }

        [HttpPost]
        public int PhatHanhVoucher(string GetId, VoucherCT voucherCT)
        {
            if (GetId != null)
            {
                var lstid = Get_Id(GetId);
                var id_0 = lstid[0];
                VoucherCT GetVoucherCTID = new VoucherCT();
                HttpResponseMessage response_VoucherCTId = _httpClient.GetAsync(_httpClient.BaseAddress + "/VoucherCT/" + id_0).Result;
                if (response_VoucherCTId.IsSuccessStatusCode)
                {
                    string jsondata = response_VoucherCTId.Content.ReadAsStringAsync().Result;
                    GetVoucherCTID = JsonConvert.DeserializeObject<VoucherCT>(jsondata);

                    // check ngày
                    Voucher Getvoucher = new Voucher();
                    HttpResponseMessage response_VoucherId = _httpClient.GetAsync(_httpClient.BaseAddress + "/Voucher/" + GetVoucherCTID.MaVoucher).Result;
                    if (response_VoucherId.IsSuccessStatusCode)
                    {
                        string jsondata1 = response_VoucherId.Content.ReadAsStringAsync().Result;
                        Getvoucher = JsonConvert.DeserializeObject<Voucher>(jsondata1);
                        if (Getvoucher.StartDate > voucherCT.NgayBatDau)
                        {
                            return 2;
                        }
                        if (Getvoucher.EndDate < voucherCT.NgayBatDau)
                        {
                            return 3;
                        }
                        if (Getvoucher.HinhThuc == 2 && voucherCT.MaKhachHang==null)
                        {
                           
                            HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer").Result;
                            List<KhachHang> lstCustome = new List<KhachHang>();
                            if (responseMessage.IsSuccessStatusCode)
                            {
                                string jsondata_lstkhachhang = responseMessage.Content.ReadAsStringAsync().Result;
                                lstCustome = JsonConvert.DeserializeObject<List<KhachHang>>(jsondata_lstkhachhang);
                                var quantityCustomer = lstCustome.Count();
                            
                                if (lstid.Count() < quantityCustomer)
                                {

                                    return 4;
                                }

                            }
                        }

                    }
                }
                List<VoucherCT> voucherCTList = new List<VoucherCT>();
                foreach (var id in lstid)
                {
                    VoucherCT cT = new VoucherCT();

                    cT.Id = id;
                    cT.MaKhachHang = voucherCT.MaKhachHang;
                    cT.NgayBatDau = voucherCT.NgayBatDau;
                    voucherCTList.Add(cT);
                }

                HttpResponseMessage response = _httpClient.PutAsJsonAsync<List<VoucherCT>>(_httpClient.BaseAddress + "/VoucherCT/PhatHanhVoucher/", voucherCTList).Result;
                if (response.IsSuccessStatusCode)
                {

                    return 1;

                }

            }
            return 0;

        }

        [HttpPost]
        public int HuyVoucher(string timCkeckBox)
        {

            var lstId = Get_Id(timCkeckBox);
            HttpResponseMessage response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "/VoucherCT/HuyVoucher/", lstId).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;

            }
            return 1;
        }

        public IActionResult DetailOrder(string Id)
        {
            HoaDon hoaDon = new HoaDon();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/VoucherCT/GetOderByIdVoucherCT/" + Id ).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                hoaDon = JsonConvert.DeserializeObject<HoaDon>(jsondata);
            }
            return Redirect("https://localhost:7047/HoaDon/ChiTiet/" + hoaDon.ID_HoaDon);
          
        }



    }
}
