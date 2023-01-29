using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin,NhanVien")]
    public class QLSachCTController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<Sach> sachs;
        private IWebHostEnvironment _hostEnviroment;
        public QLSachCTController(IWebHostEnvironment hostEnviroment)
        {
            _hostEnviroment = hostEnviroment;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
            sachs = new List<Sach>();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }

        public async Task<IActionResult> Index(string? timKiem, int? trangThai, int? page, string mess)
        {
            ViewBag.TitleAdmin = "Sách CT";
            timKiem = string.IsNullOrEmpty(timKiem) ? "" : timKiem;
            List<SachCT>? lstSach = new List<SachCT>();
            var a = await GetRequest<SachCTViewModel>("book/sachctviewmodel");
            lstSach = a.Select(c=>c.SachCT).ToList();
            //lstSach = (trangThai == 1 || trangThai == 0) ? lstSach.Where(c => c.TenSach.Contains(timKiem) && c.TrangThai == trangThai).ToList() : lstSach.Where(c => c.TenSach.Contains(timKiem)).ToList();
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            ViewBag.Sach = a;
            return View();
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

        private async Task<T> GetSingleRequest<T>(string request)
        {
            dynamic result = null;
            HttpResponseMessage responseGet = await _httpClient.GetAsync(request);
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                result = JsonConvert.DeserializeObject<T>(jsonData);
            };
            return result;
        }

        //api/nhaxuatban

        #region SelectList
        private async Task<List<SelectListItem>> GetSelectTacGia()
        {
            var lstTacGia = await GetRequest<TacGia>("api/tacgia");
            var lstItem = new List<SelectListItem>();
            lstTacGia.ForEach(x =>
            {
                var item = new SelectListItem { Text = x.HoVaTen, Value = x.ID_TacGia };
                lstItem.Add(item);
            });

            return lstItem;
        }

        private async Task<List<SelectListItem>> GetSelectTheLoai()
        {
            var lstTheLoai = await GetRequest<TheLoai>("api/theloai");
            var lstItem = new List<SelectListItem>();
            lstTheLoai.ForEach(x =>
            {
                var item = new SelectListItem { Text = x.TenTL, Value = x.ID_TheLoai };
                lstItem.Add(item);
            });

            return lstItem;
        }
        #endregion

        public async Task<IActionResult> Create()
        {
            ViewBag.NXBs = new SelectList(await GetRequest<NhaXuatBan>("api/nhaxuatban"), "ID_NXB", "TenXuatBan");
            ViewBag.Sachs = new SelectList(await GetRequest<Sach>("book"), "ID_Sach", "TenSach");
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            SachCT? sach = await GetSingleRequest<SachCT>("book/sachct/" + id);
            if (sach == null)
                return NotFound();

            ViewBag.NXBs = new SelectList(await GetRequest<NhaXuatBan>("api/nhaxuatban"), "ID_NXB", "TenXuatBan");
            //ViewBag.Sachs = new SelectList(await GetRequest<Sach>("book"), "ID_Sach", "TenSach");
            var a = await GetRequest<Sach>("book");
            ViewBag.Sach = a.FirstOrDefault(c => c.ID_Sach == sach.MaSach);

            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SachCT sach, IFormFile file)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                if (file != null && sach.GiaBan <= 5000000)
                {
                    sach.ID_SachCT = "SachCT" + Guid.NewGuid().ToString();
                    sach.TrangThai = 1;
                    sach.HinhAnh = await UpLoadFile(file);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(sach), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _httpClient.PostAsync("book/sachct", content);

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

                    if (sach.GiaBan > 5000000)
                    {
                        error = !String.IsNullOrEmpty(error) ? error + ", Giá bán không hợp lệ" : "Giá bán không hợp lệ";
                    }
                }
            }
            ViewBag.Error = error + "!";
            ViewBag.NXBs = new SelectList(await GetRequest<NhaXuatBan>("api/nhaxuatban"), "ID_NXB", "TenXuatBan");
            ViewBag.Sachs = new SelectList(await GetRequest<Sach>("book"), "ID_Sach", "TenSach");
            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SachCT sach, IFormFile? file)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                sach.HinhAnh = file != null ? await UpLoadFile(file) : sach.HinhAnh;
                StringContent content = new StringContent(JsonConvert.SerializeObject(sach), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("book/sachct", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                error = await response.Content.ReadAsStringAsync();
                error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
            }

            ViewBag.Error = error + "!";
            ViewBag.NXBs = new SelectList(await GetRequest<NhaXuatBan>("api/nhaxuatban"), "ID_NXB", "TenXuatBan");
            var a = await GetRequest<Sach>("book");
            ViewBag.Sach = a.FirstOrDefault(c => c.ID_Sach == sach.MaSach);
            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ID)
        {
            //List<Sach>? sachs = new List<Sach>();
            //sachs = await Get();
            string error = "";
            //Sach? sach = sachs?.FirstOrDefault(c => c.ID_Sach == ID);
            if (true)
            {
                //sach.TrangThai = sach.TrangThai == 1 ? 0 : 1;
                //StringContent content = new StringContent(JsonConvert.SerializeObject(sach), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("book/update_status/" + ID, null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                error = await response.Content.ReadAsStringAsync();
                error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
            }
            ViewBag.Error = error;
            return BadRequest();
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
