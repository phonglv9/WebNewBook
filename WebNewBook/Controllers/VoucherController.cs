using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebNewBook.Model;

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
    }
}
