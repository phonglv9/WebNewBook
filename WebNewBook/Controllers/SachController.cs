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
using X.PagedList;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin,NhanVien")]
    public class SachController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<Sach> sachs;
        private IWebHostEnvironment _hostEnviroment;
        public SachController(IWebHostEnvironment hostEnviroment)
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
            ViewBag.TitleAdmin = "Sách";
            timKiem = string.IsNullOrEmpty(timKiem) ? "" : timKiem;
            var pageNumber = page ?? 1;
            List<SachViewModel>? lstSach = new List<SachViewModel>();
            lstSach = await GetRequest<SachViewModel>("book/sachviewmodel");
            lstSach = lstSach.Where(c => c.Sach.TenSach.ToLower().Contains(timKiem.ToLower())).ToList();
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            ViewBag.Sach = lstSach.ToPagedList(pageNumber, 10);
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

        #region SelectList
        private async Task<List<SelectListItem>> GetSelectTacGia()
        {
            var lstTacGia = await GetRequest<TacGia>("api/tacgia");
            var lstItem = new List<SelectListItem>();
            lstTacGia.Where(c => c.TrangThai == 1).ToList().ForEach(x =>
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
            lstTheLoai.Where(c => c.TrangThai == 1).ToList().ForEach(x =>
            {
                var item = new SelectListItem { Text = x.TenTL, Value = x.ID_TheLoai };
                lstItem.Add(item);
            });

            return lstItem;
        }
        #endregion

        public async Task<IActionResult> Create()
        {
            ViewBag.TacGias = await GetSelectTacGia();
            ViewBag.TheLoais = await GetSelectTheLoai();
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            List<Sach>? sachs = new List<Sach>();
            sachs = await GetRequest<Sach>("book");
            Sach? sach = sachs?.FirstOrDefault(c => c.ID_Sach == id);
            if (sachs == null)
                return NotFound();

            var sachTG = await GetRequest<Sach_TacGia>("book/sachTG/" + sach.ID_Sach);
            var TacGias = await GetSelectTacGia();
            TacGias.ForEach(x =>
            {
                x.Selected = sachTG.Exists(c => c.MaTacGia == x.Value);
            });
            ViewBag.TacGias = TacGias;

            var sachTL = await GetRequest<Sach_TheLoai>("book/sachTL/" + sach.ID_Sach);
            var TheLoais = await GetSelectTheLoai();
            TheLoais.ForEach(x =>
            {
                x.Selected = sachTL.Exists(c => c.MaTheLoai == x.Value);
            });
            ViewBag.TheLoais = TheLoais;

            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Sach sach, string[] SelectedTacGias, string[] SelectedTheLoais, IFormFile? file)
        {
            string error = "";
            sach.ID_Sach = "Sach" + Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                if (SelectedTacGias.Length > 0 && SelectedTheLoais.Length > 0)
                {
                    SachAPI sachAPI = new SachAPI { Sach = sach, TacGias = SelectedTacGias, TheLoais = SelectedTheLoais };
                    StringContent content = new StringContent(JsonConvert.SerializeObject(sachAPI), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _httpClient.PostAsync("book", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    error = await response.Content.ReadAsStringAsync();
                    error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
                }
                else
                {
                    error = "Không được để trống tác giả hoặc thể loại!";
                }
            }
            ViewBag.Error = error;
            ViewBag.TacGias = await GetSelectTacGia();
            ViewBag.TheLoais = await GetSelectTheLoai();
            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Sach sach, string[] SelectedTacGias, string[] SelectedTheLoais, IFormFile? file)
        {
            string error = "";
            if (ModelState.IsValid)
            {
                if (SelectedTacGias.Length > 0 && SelectedTheLoais.Length > 0)
                {
                    SachAPI sachAPI = new SachAPI { Sach = sach, TacGias = SelectedTacGias, TheLoais = SelectedTheLoais };
                    StringContent content = new StringContent(JsonConvert.SerializeObject(sachAPI), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await _httpClient.PutAsync("book", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    error = await response.Content.ReadAsStringAsync();
                    error = error.Substring(error.IndexOf(":") + 1, error.IndexOf("!") - error.IndexOf(":"));
                }
                else
                {
                    error = "Không được để trống tác giả hoặc thể loại!";
                }
            }

            var sachTL = await GetRequest<Sach_TheLoai>("book/sachTL/" + sach.ID_Sach);
            var sachTG = await GetRequest<Sach_TacGia>("book/sachTG/" + sach.ID_Sach);

            ViewBag.Error = error;
            var TacGias = await GetSelectTacGia();
            TacGias.ForEach(x =>
            {
                x.Selected = sachTG.Exists(c => c.MaTacGia == x.Value);
            });
            ViewBag.TacGias = TacGias;

            var TheLoais = await GetSelectTheLoai();
            TheLoais.ForEach(x =>
            {
                x.Selected = sachTL.Exists(c => c.MaTheLoai == x.Value);
            });
            ViewBag.TheLoais = TheLoais;
            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ID)
        {
            string error = "";
            if (true)
            {
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
