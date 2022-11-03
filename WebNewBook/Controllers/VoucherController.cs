using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using WebNewBook.Model;
using WebNewBook.ViewModel;

namespace WebNewBook.Controllers
{
    public class VoucherController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;
        
        public VoucherController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;
        }


        public async Task<IActionResult> Index()
        {
            List<Voucher> Getvoucher = new List<Voucher>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Voucher" ).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                Getvoucher = JsonConvert.DeserializeObject<List<Voucher>>(jsondata);
            }
            ViewBag.lstvoucher = Getvoucher;
            return View();
        }
        public async Task<IActionResult> Add(Voucher voucher)
        {


            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(voucher), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7266/api/Voucher", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    voucher = JsonConvert.DeserializeObject<Voucher>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(string Id,VoucherModel voucherModel)
        {
            HttpResponseMessage response_1 = _httpClient.GetAsync(_httpClient.BaseAddress + "/Voucher/"+Id).Result;
            
            if (response_1.IsSuccessStatusCode )
            {
                string jsondata_1 = response_1.Content.ReadAsStringAsync().Result;
              
                voucherModel.Voucher = JsonConvert.DeserializeObject<Voucher>(jsondata_1);
             
            }
            HttpResponseMessage response_2 = _httpClient.GetAsync(_httpClient.BaseAddress + "/VoucherCT/CallIdPH/" + voucherModel.Voucher.Id).Result;

            if (response_2.IsSuccessStatusCode)
            {
                string jsondata_2 = response_2.Content.ReadAsStringAsync().Result;
                List<VoucherCT> voucherCTs = new List<VoucherCT>();
               voucherCTs= JsonConvert.DeserializeObject<List<VoucherCT>>(jsondata_2);
                ViewBag.lstvoucherCT0=voucherCTs.Where(c=>c.TrangThai==0).ToList();
                ViewBag.lstvoucherCT1=voucherCTs.Where(c=>c.TrangThai==1).ToList();

            }
         
            return View(voucherModel);
        }
        public async Task<IActionResult> CreateListvoucher(int quantityVoucher, int sizeVoucher, string startTextVoucher, string endTextVoucher, string maVoucher)
        {

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/VoucherCT/AddAutomatically?quantityVoucher=" + quantityVoucher + "&sizeVoucher=" + sizeVoucher + "&startTextVoucher=" + startTextVoucher + "&endTextVoucher=" + endTextVoucher + "&maVoucher=" + maVoucher, null).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;

            }
            return RedirectToAction("Index");
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
                        file.CopyToAsync(stream);
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

                //var filesPath =Path.Combine(Directory.GetCurrentDirectory() + "/File");
                //if (!System.IO.Directory.Exists(filesPath))
                //{
                //    Directory.CreateDirectory(filesPath);
                //}



                //var fileName = Path.GetFileName(file.FileName);
                //var filePath = Path.Combine(filesPath, fileName);

                //using (var stream = new FileStream(filesPath, FileMode.Create))
                //{
                //    await file.CopyToAsync(stream);
                //}

                string extension = Path.GetExtension(file.FileName);
                string image = file.FileName;
              string path= UpLoadFile(file, image);

                using (var httpClient = new HttpClient())
                {

                    await using var stream = System.IO.File.OpenRead(path);
                    using var request = new HttpRequestMessage(HttpMethod.Post, "file");
                    using var content = new MultipartFormDataContent {
                    { new StreamContent(stream), "file", path }
                };
                    var response = await httpClient.PostAsync("https://localhost:7266/api/VoucherCT/AddImportExcer?Phathanh=" + maPhathanh, content);
                }



                //string pathFile = file.FileName;
                //HttpClient httpClient = new HttpClient();
                //MultipartFormDataContent form = new MultipartFormDataContent();
                //HttpResponseMessage response = await httpClient.PostAsync("PostUrl", form);
                //response.EnsureSuccessStatusCode();
                //httpClient.Dispose();
                //string sd = response.Content.ReadAsStringAsync().Result;

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                return View(e.Message);
            }
        }
        public async Task<IActionResult> Create(VoucherModel voucherModel)
        {


            try
            {

                using (var httpClient = new HttpClient())
                {
                    
                    StringContent content = new StringContent(JsonConvert.SerializeObject(voucherModel.voucherCT), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:7266/api/VoucherCT/AddManually", content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        voucherModel = JsonConvert.DeserializeObject<VoucherModel>(apiResponse);
                    }
                }
                return RedirectToAction("Index");
             //   return RedirectToAction("Detail"+"/"+voucherModel.voucherCT.MaVoucher);
            }
            catch (Exception e)
            {

                return View(e.Message);
            }
          
            
        }
        public async Task<IActionResult> Update(Voucher voucher)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(voucher), Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Voucher", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                voucher = JsonConvert.DeserializeObject<Voucher>(apiResponse);
            }
            return RedirectToAction("Index");
        }

        public IActionResult  dowloadFilemau()
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
        public async Task<IActionResult> Delete(string Id)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(Id));

            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Voucher/" + Id, null).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;

            }
            return RedirectToAction("Index");
        }


        [HttpPost]
         public  int HuyVoucher(string timCkeckBox)
        {
            List<string> Id = new List<string>();
            string[] arr = timCkeckBox.Split(',');
            foreach (var x in arr)
            {
                Id.Add(x);
            }

          
            HttpResponseMessage response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "/VoucherCT/HuyVoucher/" , Id).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;

            }





            return 1;
        }
    }
}
