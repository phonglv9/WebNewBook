using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
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

        public IActionResult Create()
        {
            ViewBag.Sachs = new List<SelectListItem>
            {
                new SelectListItem {Text = "Shyju", Value = "1"},
                new SelectListItem {Text = "Sean", Value = "2"}
            };
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
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(phieuNhap), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("phieunhap", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Sachs = new List<SelectListItem>
            {
                new SelectListItem {Text = "Shyju", Value = "1"},
                new SelectListItem {Text = "Sean", Value = "2"}
            };
            return View(phieuNhap);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PhieuNhap phieuNhap)
        {
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(phieuNhap), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("phieunhap", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(phieuNhap);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ID)
        {
            List<PhieuNhap>? phieuNhaps = new List<PhieuNhap>();
            phieuNhaps = await Get();

            PhieuNhap? phieuNhap = phieuNhaps?.FirstOrDefault(c => c.ID_PhieuNhap == ID);
            if (phieuNhaps != null)
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync("phieunhap/" + ID);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest(response);
            }

            return BadRequest();

        }
    }
}
