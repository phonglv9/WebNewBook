using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QLPhieuGiamSPController : Controller
    {
        private readonly HttpClient _httpClient;
        public QLPhieuGiamSPController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }

        private async Task<List<PhieuGiamGiaSP>?> GetPG()
        {
            List<PhieuGiamGiaSP> phieu = new List<PhieuGiamGiaSP>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("PhieuGiamSP");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                phieu = JsonConvert.DeserializeObject<List<PhieuGiamGiaSP>>(jsonData);
            };
            return phieu;
        }

        public async Task<IActionResult> Index(string? timKiem, int? page, string mess)
        {
            ViewBag.TimKiem = timKiem;
            ViewBag.message = mess;
            var pageNumber = page ?? 1;
            List<PhieuGiamGiaSP>? phieu = new List<PhieuGiamGiaSP>();
            phieu = await GetPG();
            ViewBag.Phieu = phieu;
            if (!string.IsNullOrEmpty(timKiem))
            {
                timKiem = timKiem.ToLower();

                phieu = phieu.Where(c => c.TenPhieu.ToLower().Contains(timKiem)).ToList();

            }

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            List<PhieuGiamGiaSP>? phieu = new List<PhieuGiamGiaSP>();
            phieu = await GetPG();

            PhieuGiamGiaSP? pg = phieu?.FirstOrDefault(c => c.ID_PhieuGiamGiaSP == id);
            if (phieu == null)
                return NotFound();

            return View(pg);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhieuGiamGiaSP phieu)
        {
            string error = "";
            if (phieu.NgayBatDau >= phieu.NgayHetHan)
            {
                error = "Ngày không hợp lệ";
                ViewBag.Error = error;
                return View(phieu);
            }
            phieu.ID_PhieuGiamGiaSP = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(phieu), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("phieugiamsp", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Error = error;
            return View(phieu);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PhieuGiamGiaSP phieu)
        {
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(phieu), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("nhanvien", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(phieu);
        }
    }
}
