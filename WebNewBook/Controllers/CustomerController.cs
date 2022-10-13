using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
    public class CustomerController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;
        public CustomerController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;



        }
    
        public async Task<IActionResult> Index(string search)
        {
            List<KhachHang> GetKhachHang = new List<KhachHang>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Customer?search=" + search ).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
                 GetKhachHang= JsonConvert.DeserializeObject<List<KhachHang>>(jsondata);
            }
            ViewBag.lstKhachHang = GetKhachHang;   
            return View();
        }
        public  async Task<IActionResult> Add(KhachHang khachang)
        {

          
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(khachang), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:7266/api/Customer", content))
                {
                    string apiResponse= await   response.Content.ReadAsStringAsync();
                    khachang = JsonConvert.DeserializeObject<KhachHang>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> Update(KhachHang khachHang)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(khachHang), Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Customer", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string apiResponse= await response.Content.ReadAsStringAsync();
                khachHang = JsonConvert.DeserializeObject<KhachHang>(apiResponse);  
            }
           return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string Id)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(Id));
            List<KhachHang> khachHangs = new List<KhachHang>();
            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Customer/"+Id,null).Result;
            if (response.IsSuccessStatusCode)
            {
                string jsondata = response.Content.ReadAsStringAsync().Result;
              
            }
            return RedirectToAction("Index");
        }
    }
}
