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
    public class SachController : Controller
    {
        private readonly HttpClient _httpClient;
        private List<Sach> sachs;

        public SachController()
        {
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

        public async Task<IActionResult> Index()
        {
            List<Sach>? lstSach = new List<Sach>();
            lstSach = await Get();
            ViewBag.Sach = lstSach;
            return View();
        }

        private async Task<List<TacGia>> GetTacGias()
        {
            var lstTacGia = new List<TacGia>
            {
                new TacGia{ ID_TacGia = "1", HoVaTen = "Nguyễn Văn A", NgaySinh = DateTime.Now, QueQuan = "asd", TrangThai = 1},
                new TacGia{ ID_TacGia = "2", HoVaTen = "Nguyễn Văn B", NgaySinh = DateTime.Now, QueQuan = "asd", TrangThai = 1},
                new TacGia{ ID_TacGia = "3", HoVaTen = "Nguyễn Văn C", NgaySinh = DateTime.Now, QueQuan = "asd", TrangThai = 1},
                new TacGia{ ID_TacGia = "4", HoVaTen = "Nguyễn Văn D", NgaySinh = DateTime.Now, QueQuan = "asd", TrangThai = 1},
            };
            return lstTacGia;
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
            var lstTheLoai = new List<TheLoai>
            {
                new TheLoai{ID_TheLoai = "1", MaDanhMuc = "1", TenTL = "Vaughan Hardson"},
                new TheLoai{ID_TheLoai = "2", MaDanhMuc = "2", TenTL = "Elbertina Blanko"},
                new TheLoai{ID_TheLoai = "3", MaDanhMuc = "3", TenTL = "Fletch Chalk"}
            };

            return lstTheLoai;
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


        public async Task<IActionResult> Create()
        {
            ViewBag.TacGias = await GetSelectTacGia();
            ViewBag.TheLoais = await GetSelectTheLoai();
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            List<Sach>? sachs = new List<Sach>();
            sachs = await Get();

            var TacGias = await GetSelectTacGia();
            TacGias.ForEach(x =>
            {
                x.Selected = x.Value == "1" ? true : false;
            });
            ViewBag.TacGias = TacGias;

            var TheLoais = await GetSelectTheLoai();
            TheLoais.ForEach(x =>
            {
                x.Selected = x.Value == "1" ? true : false;
            });
            ViewBag.TheLoais = TheLoais;

            Sach? sach = sachs?.FirstOrDefault(c => c.ID_Sach == id);
            if (sachs == null)
                return NotFound();

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
                    ViewBag.TacGias = await GetSelectTacGia();
                    ViewBag.TheLoais = await GetSelectTheLoai();
                    return View(sach);
                }

                if (SelectedTheLoais.Length == 0)
                {
                    error = "Không được để trống thể loại!";
                    ViewBag.Error = error;
                    ViewBag.TacGias = await GetSelectTacGia();
                    ViewBag.TheLoais = await GetSelectTheLoai();
                    return View(sach);
                }

                #endregion

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
            ViewBag.TacGias = await GetSelectTacGia();
            ViewBag.TheLoais = await GetSelectTheLoai();
            return View(sach);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Sach sach, string[] SelectedTacGias, string[] SelectedTheLoais, IFormFile file)
        {
            string error = "";
            if (file == null)
            {
                error = "Hình ảnh không hợp lệ";
                ViewBag.Error = error;
                var tacGias = await GetSelectTacGia();
                tacGias.ForEach(x =>
                {
                    x.Selected = x.Value == "1" ? true : false;
                });
                ViewBag.TacGias = tacGias;

                var theLoais = await GetSelectTheLoai();
                theLoais.ForEach(x =>
                {
                    x.Selected = x.Value == "1" ? true : false;
                });
                ViewBag.TheLoais = theLoais;
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
                        x.Selected = x.Value == "1" ? true : false;
                    });
                    ViewBag.TacGias = tacGias;

                    var theLoais = await GetSelectTheLoai();
                    theLoais.ForEach(x =>
                    {
                        x.Selected = x.Value == "1" ? true : false;
                    });
                    ViewBag.TheLoais = theLoais;
                    return View(sach);
                }

                if (SelectedTheLoais.Length == 0)
                {
                    error = "Không được để trống thể loại!";
                    ViewBag.Error = error;
                    var tacGias = await GetSelectTacGia();
                    tacGias.ForEach(x =>
                    {
                        x.Selected = x.Value == "1" ? true : false;
                    });
                    ViewBag.TacGias = tacGias;

                    var theLoais = await GetSelectTheLoai();
                    theLoais.ForEach(x =>
                    {
                        x.Selected = x.Value == "1" ? true : false;
                    });
                    ViewBag.TheLoais = theLoais;
                    return View(sach);
                }

                #endregion

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
                x.Selected = x.Value == "1" ? true : false;
            });
            ViewBag.TacGias = TacGias;

            var TheLoais = await GetSelectTheLoai();
            TheLoais.ForEach(x =>
            {
                x.Selected = x.Value == "1" ? true : false;
            });
            ViewBag.TheLoais = TheLoais;
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
                HttpResponseMessage response = await _httpClient.PutAsync("book/update_status", content);
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
    }
}
