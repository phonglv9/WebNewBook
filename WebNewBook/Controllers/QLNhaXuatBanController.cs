using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
    public class QLNhaXuatBanController : Controller
    {
        private readonly HttpClient _httpClient;

        public QLNhaXuatBanController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/api/");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }
        private async Task<List<NhaXuatBan>?> GetNXB()
        {
            List<NhaXuatBan> nhaXuatBans = new List<NhaXuatBan>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("NhaXuatBan");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                nhaXuatBans = JsonConvert.DeserializeObject<List<NhaXuatBan>>(jsonData);
            };
            return nhaXuatBans;
        }
        public async Task<IActionResult> Index(string? timKiem, int trangThai)
        {
            List<NhaXuatBan>? nhaXuatBans = new List<NhaXuatBan>();
            nhaXuatBans = await GetNXB();
            if (!string.IsNullOrEmpty(timKiem))
            {
                timKiem = timKiem.ToLower();
                nhaXuatBans = nhaXuatBans.Where(c => c.TenXuatBan.ToLower().Contains(timKiem)).ToList();
               
            }
            ViewBag.NhaXuatBan = nhaXuatBans;
            return View();
            
        }
    }
}
