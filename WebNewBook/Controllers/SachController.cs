using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;

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

        public async Task<List<Sach>?> Get()
        {
            List<Sach> sachs = new List<Sach>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("book");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sachs = JsonConvert.DeserializeObject<List<Sach>>(jsonData);
            };
            return sachs;
        }

        public async Task<IActionResult> Index(string? timKiem, int? trangThai, int? page, string mess)
        {
            ViewBag.TitleAdmin = "Sách";
            timKiem = string.IsNullOrEmpty(timKiem) ? "" : timKiem;
            List<Sach>? lstSach = new List<Sach>();
            lstSach = await Get();
            lstSach = (trangThai == 1 || trangThai == 0) ? lstSach.Where(c => c.TenSach.Contains(timKiem) && c.TrangThai == trangThai).ToList() : lstSach.Where(c => c.TenSach.Contains(timKiem)).ToList();
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            ViewBag.Sach = lstSach;
            return View();
        }

        private async Task<List<TacGia>> GetTacGias()
        {
            List<TacGia> sachs = new List<TacGia>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("api/tacgia");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sachs = JsonConvert.DeserializeObject<List<TacGia>>(jsonData);
            };
            return sachs;
        }
        private async Task<List<SelectListItem>> GetSelectTacGia()
        {
            var lstTacGia = await GetTacGias();
            var lstItem = new List<SelectListItem>();
            lstTacGia.ForEach(x =>
            {
                var item = new SelectListItem { Text = x.HoVaTen, Value = x.ID_TacGia };
                lstItem.Add(item);
            });

            return lstItem;
        }
        private async Task<List<TheLoai>> GetTheLoais()
        {
            List<TheLoai> sachs = new List<TheLoai>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("api/theloai");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sachs = JsonConvert.DeserializeObject<List<TheLoai>>(jsonData);
            };
            return sachs;
        }
        private async Task<List<SelectListItem>> GetSelectTheLoai()
        {
            var lstTheLoai = await GetTheLoais();
            var lstItem = new List<SelectListItem>();
            lstTheLoai.ForEach(x =>
            {
                var item = new SelectListItem { Text = x.TenTL, Value = x.ID_TheLoai };
                lstItem.Add(item);
            });

            return lstItem;
        }

        private async Task<List<NhaXuatBan>> GetNhaXuatBans()
        {
            List<NhaXuatBan> sachs = new List<NhaXuatBan>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("api/nhaxuatban");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sachs = JsonConvert.DeserializeObject<List<NhaXuatBan>>(jsonData);
            };
            return sachs;
        }

        private async Task<List<SachCT>> GetSachCts(string Id)
        {
            List<SachCT> sachs = new List<SachCT>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("book/sachCT/" + Id);
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                sachs = JsonConvert.DeserializeObject<List<SachCT>>(jsonData);
            };
            return sachs;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan");
            ViewBag.TacGias = await GetSelectTacGia();
            ViewBag.TheLoais = await GetSelectTheLoai();
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            List<Sach>? sachs = new List<Sach>();
            sachs = await Get();
            Sach? sach = sachs?.FirstOrDefault(c => c.ID_Sach == id);
            if (sachs == null)
                return NotFound();

            var sachCTs = await GetSachCts(sach.ID_Sach);

            var TacGias = await GetSelectTacGia();
            TacGias.ForEach(x =>
            {
                x.Selected = sachCTs.Exists(c => c.MaTacGia == x.Value);
            });
            ViewBag.TacGias = TacGias;

            var TheLoais = await GetSelectTheLoai();
            TheLoais.ForEach(x =>
            {
                x.Selected = sachCTs.Exists(c => c.MaTheLoai == x.Value);
            });
            ViewBag.TheLoais = TheLoais;

            ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan", sach.MaNXB);

            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Sach sach, string[] SelectedTacGias, string[] SelectedTheLoais, IFormFile file)
        {
            string error = "";
            if (file == null)
            {
                error = "Hình ảnh không hợp lệ";
                ViewBag.Error = error;
                ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan");
                ViewBag.TacGias = await GetSelectTacGia();
                ViewBag.TheLoais = await GetSelectTheLoai();
                return View(sach);
            }
            if (sach.GiaBan > 5000000)
            {
                error = "Giá bán không hợp lệ";
                ViewBag.Error = error;
                ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan");
                ViewBag.TacGias = await GetSelectTacGia();
                ViewBag.TheLoais = await GetSelectTheLoai();
                return View(sach);

            }

            sach.ID_Sach = "Sach" + Guid.NewGuid().ToString();
            sach.TrangThai = 1;
            sach.HinhAnh = file.FileName;
            if (ModelState.IsValid)
            {
                #region validate select
                if (SelectedTacGias.Length == 0)
                {
                    error = "Không được để trống tác giả!";
                    ViewBag.Error = error;
                    ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan");
                    ViewBag.TacGias = await GetSelectTacGia();
                    ViewBag.TheLoais = await GetSelectTheLoai();
                    return View(sach);
                }

                if (SelectedTheLoais.Length == 0)
                {
                    error = "Không được để trống thể loại!";
                    ViewBag.Error = error;
                    ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan");
                    ViewBag.TacGias = await GetSelectTacGia();
                    ViewBag.TheLoais = await GetSelectTheLoai();
                    return View(sach);
                }

                #endregion

                sach.HinhAnh = await UpLoadFile(file);
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
            ViewBag.Error = error;
            ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan");
            ViewBag.TacGias = await GetSelectTacGia();
            ViewBag.TheLoais = await GetSelectTheLoai();
            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Sach sach, string[] SelectedTacGias, string[] SelectedTheLoais, IFormFile? file)
        {
            var sachCTs = await GetSachCts(sach.ID_Sach);
            string error = "";
            if (file == null && sach.HinhAnh == string.Empty)
            {
                error = "Hình ảnh không hợp lệ";
                ViewBag.Error = error;
                var tacGias = await GetSelectTacGia();
                tacGias.ForEach(x =>
                {
                    x.Selected = sachCTs.Exists(c => c.MaTacGia == x.Value);
                });
                ViewBag.TacGias = tacGias;

                var theLoais = await GetSelectTheLoai();
                theLoais.ForEach(x =>
                {
                    x.Selected = sachCTs.Exists(c => c.MaTheLoai == x.Value);
                });
                ViewBag.TheLoais = theLoais;
                ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan", sach.MaNXB);
                return View(sach);
            }
            if (ModelState.IsValid)
            {
                #region validate select
                if (SelectedTacGias.Length == 0)
                {
                    error = "Không được để trống tác giả!";
                    ViewBag.Error = error;
                    var tacGias = await GetSelectTacGia();
                    tacGias.ForEach(x =>
                    {
                        x.Selected = sachCTs.Exists(c => c.MaTacGia == x.Value);
                    });
                    ViewBag.TacGias = tacGias;

                    var theLoais = await GetSelectTheLoai();
                    theLoais.ForEach(x =>
                    {
                        x.Selected = sachCTs.Exists(c => c.MaTheLoai == x.Value);
                    });
                    ViewBag.TheLoais = theLoais;
                    ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan", sach.MaNXB);
                    return View(sach);
                }

                if (SelectedTheLoais.Length == 0)
                {
                    error = "Không được để trống thể loại!";
                    ViewBag.Error = error;
                    var tacGias = await GetSelectTacGia();
                    tacGias.ForEach(x =>
                    {
                        x.Selected = sachCTs.Exists(c => c.MaTacGia == x.Value);
                    });
                    ViewBag.TacGias = tacGias;

                    var theLoais = await GetSelectTheLoai();
                    theLoais.ForEach(x =>
                    {
                        x.Selected = sachCTs.Exists(c => c.MaTheLoai == x.Value);
                    });
                    ViewBag.TheLoais = theLoais;
                    ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan", sach.MaNXB);
                    return View(sach);
                }

                #endregion

                sach.HinhAnh = file != null ? await UpLoadFile(file) : sach.HinhAnh;
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
            ViewBag.Error = error;
            var TacGias = await GetSelectTacGia();
            TacGias.ForEach(x =>
            {
                x.Selected = sachCTs.Exists(c => c.MaTacGia == x.Value);
            });
            ViewBag.TacGias = TacGias;

            var TheLoais = await GetSelectTheLoai();
            TheLoais.ForEach(x =>
            {
                x.Selected = sachCTs.Exists(c => c.MaTheLoai == x.Value);
            });
            ViewBag.TheLoais = TheLoais;
            ViewBag.NXBs = new SelectList(await GetNhaXuatBans(), "ID_NXB", "TenXuatBan", sach.MaNXB);
            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string ID)
        {
            List<Sach>? sachs = new List<Sach>();
            sachs = await Get();
            string error = "";
            Sach? sach = sachs?.FirstOrDefault(c => c.ID_Sach == ID);
            if (sachs != null)
            {
                sach.TrangThai = sach.TrangThai == 1 ? 0 : 1;
                StringContent content = new StringContent(JsonConvert.SerializeObject(sach), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync("book/update_status/" + ID, content);
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
