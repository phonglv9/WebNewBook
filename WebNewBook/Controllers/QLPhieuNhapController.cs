using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;

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

        private async Task<List<T>> GetRequest<T>(string request)
        {
            List<T> result = new List<T>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync(request);
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<List<T>>(jsonData);
            };
            return result;
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
            lstPhieuNhap = await GetRequest<PhieuNhap>("phieunhap");
            var sach = await GetRequest<SachCTViewModel>("book/sachctviewmodel");
            List<PhieuNhapViewModel> phieuNhapViewModels = new List<PhieuNhapViewModel>();
            lstPhieuNhap.ForEach(c =>
            {
                var book = sach.FirstOrDefault(x => x.SachCT.ID_SachCT == c.MaSachCT);
                var loaiBia = book.SachCT.BiaMem ? "Bìa mềm" : "Bìa cứng";
                PhieuNhapViewModel phieuNhapViewModel = new PhieuNhapViewModel { PhieuNhap = c, Sach = book.TenSach + " - Tái bản: " + book.SachCT.TaiBan + " - NXB: " + book.NXB + " - Loại bìa: " + loaiBia};
                phieuNhapViewModels.Add(phieuNhapViewModel);
            });
            ViewBag.PhieuNhap = phieuNhapViewModels;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var sachs = await GetRequest<SachCTViewModel>("book/sachctviewmodel");
            var selectItems = new List<SelectListItem>();
            sachs.ForEach(s =>
            {
                var loaiBia = s.SachCT.BiaMem ? "Bìa mềm" : "Bìa cứng";
                var a = new SelectListItem { Text = s.TenSach + " - Tái bản: " + s.SachCT.TaiBan + " - NXB: " + s.NXB + " - Loại bìa: " + loaiBia, Value = s.SachCT.ID_SachCT };
                selectItems.Add(a);
            });
            //selectItems = sachs.Select(s => new SelectListItem { Text = s.TenSach + " - Tái bản: " + s.SachCT.TaiBan + " - NXB: " + s.NXB + " - Loại bìa: " +s.SachCT.BiaMem?"":"", Value = s.SachCT.ID_SachCT }).ToList();
            ViewBag.Sachs = selectItems;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PhieuNhap phieuNhap)
        {

            phieuNhap.MaNhanVien = User.Claims.FirstOrDefault(c => c.Type == "Id").Value;
            phieuNhap.ID_PhieuNhap = "PN" + Guid.NewGuid().ToString();
            string error = "";

            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(phieuNhap), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("phieunhap", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            var sachs = await GetRequest<SachCTViewModel>("book/sachctviewmodel");
            var selectItems = new List<SelectListItem>();
            sachs.ForEach(s =>
            {
                var loaiBia = s.SachCT.BiaMem ? "Bìa mềm" : "Bìa cứng";
                var a = new SelectListItem { Text = s.TenSach + " - Tái bản: " + s.SachCT.TaiBan + " - NXB: " + s.NXB + " - Loại bìa: " + loaiBia, Value = s.SachCT.ID_SachCT };
                selectItems.Add(a);
            });
            ViewBag.Sachs = selectItems;
            return View(phieuNhap);
        }
    }
}
