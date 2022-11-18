using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.API.Book;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "NhanVien")]
    public class QLPhieuNhapController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<PhieuNhap> phieuNhaps;
        public QLPhieuNhapController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
            phieuNhaps = new List<PhieuNhap>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }

        private async Task<List<BookModel>> GetSachs()
        {
            List<BookModel> sachs = new List<BookModel>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("api/Book/GetlistBook");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sachs = JsonConvert.DeserializeObject<List<BookModel>>(jsonData);
            };
            return sachs ?? new List<BookModel>();
        }

        public async Task<List<PhieuNhap>?> Get()
        {
            List<PhieuNhap> phieuNhaps = new List<PhieuNhap>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("phieunhap");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                phieuNhaps = JsonConvert.DeserializeObject<List<PhieuNhap>>(jsonData);
            };
            return phieuNhaps;
        }

        public async Task<IActionResult> Index()
        {
            List<PhieuNhap>? lstPhieuNhap = new List<PhieuNhap>();
            lstPhieuNhap = await Get();
            ViewBag.PhieuNhap = lstPhieuNhap;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var sachs = await GetSachs();
            var selectItems = new List<SelectListItem>();
            selectItems = sachs.Select(s => new SelectListItem { Text = s.TenSach, Value = s.ID_Sach }).ToList();
            ViewBag.Sachs = selectItems;
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            List<PhieuNhap>? phieuNhaps = new List<PhieuNhap>();
            phieuNhaps = await Get();

            PhieuNhap? phieuNhap = phieuNhaps?.FirstOrDefault(c => c.ID_PhieuNhap == id);
            if (phieuNhaps == null)
                return NotFound();

            return View(phieuNhap);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhieuNhap phieuNhap)
        {
            phieuNhap.MaNhanVien = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(phieuNhap), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("phieunhap", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            var sachs = await GetSachs();
            var selectItems = new List<SelectListItem>();
            selectItems = sachs.Select(s => new SelectListItem { Text = s.TenSach, Value = s.ID_Sach }).ToList();
            ViewBag.Sachs = selectItems;
            return View(phieuNhap);
        }
    }
}
