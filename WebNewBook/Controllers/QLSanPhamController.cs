using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;
using X.PagedList;
using System.IO;
using static WebNewBook.API.Repository.Service.BookService;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin,NhanVien")]
    public class QLSanPhamController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<SanPham> sanPhams;
        private IWebHostEnvironment _hostEnviroment;
        public QLSanPhamController(IWebHostEnvironment hostEnvironment)
        {
            this._hostEnviroment = hostEnvironment;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
            sanPhams = new List<SanPham>();
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

        private async Task<List<SanPham>?> Get()
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


        private async Task<List<SelectListItem>> GetSelectListItems()
        {
            var sachs = await GetRequest<Sach_SachCT>("book/sach_sachct");
            var listItem = new List<SelectListItem>();
            sachs.ForEach(s =>
            {
                var loaiBia = s.SachCT.BiaMem ? "Bìa mềm" : "Bìa cứng";
                listItem.Add(new SelectListItem { Text = s.TenSach + " - " + "NXB: " + s.NXB + " - " + "Tái bản " + s.SachCT.TaiBan + " - " + loaiBia + " - " + s.SachCT.GiaBan, Value = s.SachCT.ID_SachCT + " @ " + s.SachCT.GiaBan });
            });
            return listItem;
        }
        public async Task<IActionResult> Index(string? timKiem, int? trangThai, int? page, string mess)
        {
            ViewBag.TitleAdmin = "Sản phẩm";
            timKiem = string.IsNullOrEmpty(timKiem) ? "" : timKiem;
            List<SanPhamViewModel>? lstSanPham = new List<SanPhamViewModel>();
            lstSanPham = await GetRequest<SanPhamViewModel>("sanpham/viewmodel");
            lstSanPham = (trangThai == 1 || trangThai == 0) ? lstSanPham.Where(c => c.SanPham.TenSanPham.Contains(timKiem) && c.SanPham.TrangThai == trangThai).OrderByDescending(c => c.SanPham.NgayTao).ToList() : lstSanPham.Where(c => c.SanPham.TenSanPham.Contains(timKiem)).OrderByDescending(c => c.SanPham.NgayTao).ToList();
            ViewBag.SanPham = lstSanPham;
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            var pageNumber = page ?? 1;
            ViewBag.NXB = lstSanPham.ToPagedList(pageNumber, 5);

            return View();
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Sachs = await GetSelectListItems();
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            List<SanPham>? sanPhams = new List<SanPham>();
            sanPhams = await GetRequest<SanPham>("sanpham");

            SanPham? sanPham = sanPhams?.FirstOrDefault(c => c.ID_SanPham == id);
            if (sanPhams == null)
                return NotFound();

            SanPhamAPI sanPhamAPI = new SanPhamAPI();
            sanPhamAPI.SanPham = sanPham;
            var sachs = await GetRequest<SachCTViewModel>("sanpham/sanpham_sachct/" + id);

            ViewBag.Saches = sachs;

            //double giaGoc = 0;
            //foreach (var item in sachs)
            //{
            //    giaGoc += item.GiaBan;
            //}
            sanPhamAPI.GiamGia = 100 - sanPham.GiaBan * 100 / sanPham.GiaGoc;
            return View(sanPhamAPI);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SanPhamAPI sanPhamAPI, string[] SelectedSachs, IFormFile file)
        {
            string error = "";
            sanPhamAPI.SanPham.ID_SanPham = "SP" + Guid.NewGuid().ToString();
            sanPhamAPI.Sachs = SelectedSachs.Select(c => c.Substring(0, c.IndexOf("@") - 1));
            sanPhamAPI.SanPham.NgayTao = DateTime.Now;
            sanPhamAPI.SLSachCT = SelectedSachs.Length > 1 ? 1 : sanPhamAPI.SLSachCT;

            if (ModelState.IsValid)
            {
                if (file != null && SelectedSachs.Length > 0 /*&& ((SelectedSachs.Length == 1 && sanPhamAPI.SLSachCT >= 1) || (SelectedSachs.Length > 1 && sanPhamAPI.SLSachCT == 1))*/)
                {
                    double giaBan = 0;
                    foreach (var item in SelectedSachs)
                    {
                        var gia = item.Trim().Substring(item.IndexOf("@") + 1);
                        giaBan += double.Parse(gia);
                    }

                    sanPhamAPI.SanPham.HinhAnh = await UpLoadFile(file);
                    sanPhamAPI.SanPham.GiaGoc = giaBan * sanPhamAPI.SLSachCT;
                    sanPhamAPI.SanPham.GiaBan = giaBan * sanPhamAPI.SLSachCT - giaBan * sanPhamAPI.SLSachCT * (sanPhamAPI.GiamGia / 100);
                    sanPhamAPI.SanPham.TrangThai = 1;

                    StringContent content = new StringContent(JsonConvert.SerializeObject(sanPhamAPI), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _httpClient.PostAsync("sanpham", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                    error = await response.Content.ReadAsStringAsync();
                    error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
                }
                else
                {
                    if (file == null)
                    {
                        error += "Hình ảnh không hợp lệ";
                    }

                    if (SelectedSachs.Length == 0)
                    {
                        error = !String.IsNullOrEmpty(error) ? error + ", Sách hoặc bộ sách không hợp lệ!" : "Sách hoặc bộ sách không hợp lệ!";
                    }
                }
            }

            ViewBag.Sachs = await GetSelectListItems();
            ViewBag.Error = error;
            return View(sanPhamAPI);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SanPhamAPI sanPhamAPI, IFormFile? file)
        {
            var sachs = await GetRequest<SachCTViewModel>("sanpham/sanpham_sachct/" + sanPhamAPI.SanPham.ID_SanPham);
            sanPhamAPI.Sachs = sachs.Select(c => c.SachCT.ID_SachCT);
            string error = "";
            //if (file == null && sanPhamAPI.SanPham.HinhAnh == string.Empty)
            //{
            //    error = "Hình ảnh không hợp lệ";
            //    ViewBag.Sachs = sachs;
            //    ViewBag.Error = error;
            //    return View(sanPhamAPI);
            //}

            if (ModelState.IsValid)
            {
                if (sanPhamAPI.SanPham.HinhAnh != string.Empty)
                {
                    sanPhamAPI.SanPham.HinhAnh = file != null ? await UpLoadFile(file) : sanPhamAPI.SanPham.HinhAnh;
                    double giaBan = sanPhamAPI.SanPham.GiaGoc - sanPhamAPI.SanPham.GiaGoc * (sanPhamAPI.GiamGia / 100);
                    //foreach (var item in sachs)
                    //{
                    //    giaBan += item.GiaBan;
                    //}

                    sanPhamAPI.SanPham.GiaBan = giaBan;
                    sanPhamAPI.SanPham.TrangThai = 1;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(sanPhamAPI), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _httpClient.PutAsync("sanpham", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }

                    error = await response.Content.ReadAsStringAsync();
                    error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
                }
                else
                {
                    error = "Hình ảnh không hợp lệ";
                }
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
                HttpResponseMessage response = await _httpClient.PutAsync("sanpham/update_status", content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return BadRequest(response);
            }

            return BadRequest();
        }

        public IActionResult SachDetail(string id)
        {
            return RedirectToAction("Update", "Sach", id);
        }

        private async Task<string> UpLoadFile(IFormFile file)
        {
            string rootPath = _hostEnviroment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string path = Path.Combine(rootPath + @"\img\", fileName + extension);

            if (!System.IO.File.Exists(path))
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                };
            }

            return path.Replace(rootPath + @"\img\", string.Empty);
        }
    }
}
