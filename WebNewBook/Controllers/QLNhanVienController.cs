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
    public class QLNhanVienController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<NhanVien> nhanViens;
        private static Random random = new Random();
        public QLNhanVienController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
            nhanViens = new List<NhanVien>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }

        public async Task<List<NhanVien>?> Get()
        {
            List<NhanVien> nhanViens = new List<NhanVien>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("nhanvien");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                nhanViens = JsonConvert.DeserializeObject<List<NhanVien>>(jsonData);
            };
            return nhanViens;
        }

        public async Task<IActionResult> Index(string? timKiem, int? trangThai, int? page, string mess)
        {
            timKiem = string.IsNullOrEmpty(timKiem) ? "" : timKiem;
            List<NhanVien>? lstNhanVien = new List<NhanVien>();
            lstNhanVien = await Get();
            lstNhanVien = (trangThai == 1 || trangThai == 0) ? lstNhanVien.Where(c => c.HoVaTen.Contains(timKiem) && c.TrangThai == trangThai).ToList() : lstNhanVien.Where(c => c.HoVaTen.Contains(timKiem)).ToList();
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            ViewBag.NhanVien = lstNhanVien;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            List<NhanVien>? nhanViens = new List<NhanVien>();
            nhanViens = await Get();

            NhanVien? nhanVien = nhanViens?.FirstOrDefault(c => c.ID_NhanVien == id);
            if (nhanViens == null)
                return NotFound();

            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhanVien nhanVien)
        {
            string error = "";
            nhanVien.ID_NhanVien = "NV" + Guid.NewGuid().ToString();
            nhanVien.TrangThai = 1;
            nhanVien.MatKhau = "NV@12345" + nhanVien.Email;
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(nhanVien), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("nhanvien", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                error = await response.Content.ReadAsStringAsync();
            }

            ViewBag.Error = error;
            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> Update(NhanVien nhanVien)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(nhanVien), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("nhanvien", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                error = await response.Content?.ReadAsStringAsync();
            }

            ViewBag.Error = error;
            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ID)
        {
            List<NhanVien>? nhanViens = new List<NhanVien>();
            nhanViens = await Get();

            NhanVien? nhanVien = nhanViens?.FirstOrDefault(c => c.ID_NhanVien == ID);
            if (nhanViens != null)
            {
                nhanVien.TrangThai = nhanVien.TrangThai == 1 ? 0 : 1;
                StringContent content = new StringContent(JsonConvert.SerializeObject(nhanVien), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("nhanvien/", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest(response);
            }

            return BadRequest();

        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
