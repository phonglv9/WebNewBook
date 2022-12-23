using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QLPhieuNhapController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<PhieuNhap> phieuNhaps;
        public class PhieuNhapViewModel
        {
            public PhieuNhap PhieuNhap { get; set; }
            public string Sach { get; set; }
        }

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

        private async Task<List<Sach>> GetSachs()
        {
            List<Sach> sachs = new List<Sach>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("book");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sachs = JsonConvert.DeserializeObject<List<Sach>>(jsonData);
            };
            return sachs ?? new List<Sach>();
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
            ViewBag.TitleAdmin = "Phiếu nhập";
            List<PhieuNhap>? lstPhieuNhap = new List<PhieuNhap>();
            lstPhieuNhap = await Get();
            var sach = await GetSachs();
            List<PhieuNhapViewModel> phieuNhapViewModels = new List<PhieuNhapViewModel>();
            lstPhieuNhap.ForEach(c =>
            {
                var book = sach.FirstOrDefault(x => x.ID_Sach == c.MaSach);
                PhieuNhapViewModel phieuNhapViewModel = new PhieuNhapViewModel { PhieuNhap = c, Sach = book.TenSach + " - Tái bản: " + book.TaiBan };
                phieuNhapViewModels.Add(phieuNhapViewModel);
            });
            ViewBag.PhieuNhap = phieuNhapViewModels;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var sachs = await GetSachs();
            var selectItems = new List<SelectListItem>();
            selectItems = sachs.Select(s => new SelectListItem { Text = s.TenSach + " - Tai ban: " + s.TaiBan, Value = s.ID_Sach }).ToList();
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
            phieuNhap.ID_PhieuNhap = "PN" + Guid.NewGuid().ToString();
            var sachs = await GetSachs();
            string error = "";
            var selectItems = new List<SelectListItem>();
            //if (sachs.FirstOrDefault(c => c.ID_Sach == phieuNhap.MaSach).GiaBan < phieuNhap.GiaNhap)
            //{
            //    error = "Giá nhập không vượt quá " + sachs.FirstOrDefault(c => c.ID_Sach == phieuNhap.MaSach).GiaBan.ToString();
            //    ViewBag.Error = error;
            //    selectItems = sachs.Select(s => new SelectListItem { Text = s.TenSach, Value = s.ID_Sach }).ToList();
            //    ViewBag.Sachs = selectItems;
            //    return View(phieuNhap);
            //}

            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(phieuNhap), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("phieunhap", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            selectItems = sachs.Select(s => new SelectListItem { Text = s.TenSach, Value = s.ID_Sach }).ToList();
            ViewBag.Sachs = selectItems;
            return View(phieuNhap);
        }
    }
}
