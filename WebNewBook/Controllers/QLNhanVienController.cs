using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebNewBook.Model;

namespace WebNewBook.Controllers
{
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

        public async Task<IActionResult> Index()
        {
            List<NhanVien>? lstNhanVien = new List<NhanVien>();
            lstNhanVien = await Get();
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
            nhanVien.ID_NhanVien = "NV" + Guid.NewGuid().ToString();
            nhanVien.TrangThai = 0;
            nhanVien.MatKhau = RandomString(10);
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(nhanVien), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("nhanvien", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> Update(NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(nhanVien), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("nhanvien", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
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
