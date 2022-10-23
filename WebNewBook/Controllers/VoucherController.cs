using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                ViewBag.lstvoucherCT=voucherCTs;

            }
         
            return View(voucherModel);
        }
        //public async Task<IActionResult> CreateListvoucher(int quantityVoucher, int sizeVoucher, string startTextVoucher, string endTextVoucher, string maVoucher)
        //{
        //    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/VoucherCT/AddAutomatically?" + status + "&search=" + search).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsondata = response.Content.ReadAsStringAsync().Result;
        //        GetKhachHang = JsonConvert.DeserializeObject<List<KhachHang>>(jsondata);
        //    }
        //    ViewBag.lstKhachHang = GetKhachHang;
        //    return View();
        //}
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

        //public async Task<IActionResult> ThemtungVoucher(VoucherCT voucherCT)
        //{

        //}
    }
}
