using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;

namespace WebNewBook.Controllers
{
    public class QLSanPhamController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<SanPham> sanPhams;
        public QLSanPhamController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
            sanPhams = new List<SanPham>();
        }

        public async Task<List<SanPham>?> Get()
        {
            List<SanPham> sanPhams = new List<SanPham>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("sanpham");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sanPhams = JsonConvert.DeserializeObject<List<SanPham>>(jsonData);
            };
            return sanPhams;
        }

        public async Task<IActionResult> Index()
        {
            List<SanPham>? lstSanPham = new List<SanPham>();
            lstSanPham = await Get();
            ViewBag.SanPham = lstSanPham;
            return View();
        }

        public async Task<List<Sach>?> GetSachs(string id)
        {
            List<Sach> sachs = new List<Sach>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("sanpham/sanpham_sach/"+id);
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sachs = JsonConvert.DeserializeObject<List<Sach>>(jsonData);
            };
            return sachs;
        }

        public IActionResult Create()
        {
            ViewBag.Sachs = new List<SelectListItem>
            {
                new SelectListItem {Text = "Shyju - 10000", Value = "3 @ 10000"},
                new SelectListItem {Text = "Sean - 30000", Value = "4 @ 30000"}
            };

            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            List<SanPham>? sanPhams = new List<SanPham>();
            sanPhams = await Get();

            SanPham? sanPham = sanPhams?.FirstOrDefault(c => c.ID_SanPham == id);
            if (sanPhams == null)
                return NotFound();

            SanPhamAPI sanPhamAPI = new SanPhamAPI();
            sanPhamAPI.SanPham = sanPham;
            var sachs = await GetSachs(sanPham.ID_SanPham);
            
            ViewBag.Saches = sachs;

            double giaGoc = 0;
            foreach (var item in sachs)
            {
                giaGoc += item.GiaBan;
            }
            sanPhamAPI.GiamGia = 100 - sanPham.GiaBan * 100 / giaGoc;
            return View(sanPhamAPI);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SanPhamAPI sanPhamAPI, string[] SelectedSachs)
        {
            sanPhamAPI.SanPham.ID_SanPham = "SP" + Guid.NewGuid().ToString();
            sanPhamAPI.Sachs = SelectedSachs;
            string error = "";
            if (ModelState.IsValid)
            {
                double giaBan = 0;
                foreach (var item in sanPhamAPI.Sachs)
                {
                    var gia = item.Trim().Substring(item.IndexOf("@") + 1);
                    giaBan += double.Parse(gia);
                }

                sanPhamAPI.SanPham.GiaBan = giaBan - giaBan*(sanPhamAPI.GiamGia/100);

                StringContent content = new StringContent(JsonConvert.SerializeObject(sanPhamAPI), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("sanpham", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                error = await response.Content.ReadAsStringAsync();
                error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
            }

            ViewBag.Sachs = new List<SelectListItem>
            {
                new SelectListItem {Text = "Shyju - 10000", Value = "3 @ 10000"},
                new SelectListItem {Text = "Sean - 30000", Value = "4 @ 30000"}
            };
            ViewBag.Error = error;
            return View(sanPhamAPI);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SanPhamAPI sanPhamAPI)
        {
            var sachs = await GetSachs(sanPhamAPI.SanPham.ID_SanPham);
            sanPhamAPI.Sachs = sachs.Select(c => c.ID_Sach);
            string error = "";
            if (ModelState.IsValid)
            {
                double giaBan = 0;
                foreach (var item in sachs)
                {
                    giaBan += item.GiaBan;
                }

                sanPhamAPI.SanPham.GiaBan = giaBan - giaBan * (sanPhamAPI.GiamGia / 100);
                StringContent content = new StringContent(JsonConvert.SerializeObject(sanPhamAPI.SanPham), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("sanpham", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                error = await response.Content.ReadAsStringAsync();
                error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
            }

            ViewBag.Saches = sachs;
            ViewBag.Error = error;
            return View(sanPhamAPI);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ID)
        {
            List<SanPham>? sanPhams = new List<SanPham>();
            sanPhams = await Get();

            SanPham? sanPham = sanPhams?.FirstOrDefault(c => c.ID_SanPham == ID);
            if (sanPhams != null)
            {
                sanPham.TrangThai = sanPham.TrangThai == 1 ? 0 : 1;
                StringContent content = new StringContent(JsonConvert.SerializeObject(sanPham), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("sanpham/", content);
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
