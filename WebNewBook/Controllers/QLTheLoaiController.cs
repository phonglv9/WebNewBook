using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.Model;
using X.PagedList;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QLTheLoaiController : Controller
    {
        private readonly HttpClient _httpClient;

        public QLTheLoaiController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }
        private async Task<List<TheLoai>?> GetTL()
        {
            List<TheLoai> theLoais = new List<TheLoai>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("api/TheLoai");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                theLoais = JsonConvert.DeserializeObject<List<TheLoai>>(jsonData);
            };
            return theLoais;
        }
        private async Task<List<DanhMucSach>?> GetDanhMuc()
        {
            List<DanhMucSach> danhMucSaches = new List<DanhMucSach>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("api/DanhMucSach");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                danhMucSaches = JsonConvert.DeserializeObject<List<DanhMucSach>>(jsonData);
            };
            return danhMucSaches;
        }
        //GET: Saches/Create
        public class MultiDropDownListViewModel
        {
            public string? Id { get; set; }
            public string ?Name { get; set; }
            public List<SelectListItem>? ItemList { get; set; }
        }
        public async Task<IActionResult> Index(string? timKiem, int? trangThai, int? page, string mess)
        {
            ViewBag.TitleAdmin = "Thể Loại";
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            var pageNumber = page ?? 1;
          
            MultiDropDownListViewModel model = new();
            var  theLoais = await GetTL();
            var danhMucs = await GetDanhMuc();
            if (!string.IsNullOrEmpty(timKiem))
            {
                timKiem = timKiem.ToLower();

                theLoais = theLoais.Where(c => c.TenTL.ToLower().Contains(timKiem)).ToList();

            }
            if (trangThai != 0)
            {
                switch (trangThai)
                {
                    case 1:
                        theLoais = theLoais.Where(c => c.TrangThai == 1).ToList();

                        break;


                    case 2:
                        theLoais = theLoais.Where(c => c.TrangThai == 0).ToList();
                        break;

                    default:
                        theLoais = theLoais.ToList();
                        break;
                }
            }
           
            ViewBag.DanhMuc = model.ItemList = danhMucs.Select(x => new SelectListItem { Text = x.TenDanhMuc, Value = x.ID_DanhMuc.ToString() }).ToList();
            ViewBag.TL = theLoais.ToPagedList(pageNumber, 15);
            return View();

        }
        public async Task<IActionResult> AddTL(TheLoai theLoai)
        {
            theLoai.ID_TheLoai = "TL" + DateTime.Now.Ticks;
            theLoai.TrangThai = 1;
            if (theLoai != null)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(theLoai), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/TheLoai", content);


                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index", new { mess = 1 });
                }
                else
                {

                    return RedirectToAction("Index", new { mess = 2 });
                }



            }
            else
            {

                return RedirectToAction("Index", new { mess = 2 });
            }


        }
        public async Task<IActionResult> UpdateTL(TheLoai theLoai)
        {
            if (theLoai != null)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(theLoai), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/TheLoai", content);


                if (response.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index", new { mess = 1 });
                }
                else
                {

                    return RedirectToAction("Index", new { mess = 2 });
                }



            }
            else
            {

                return RedirectToAction("Index", new { mess = 2 });
            }


        }
        public async Task<IActionResult> RemoveTL(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                HttpResponseMessage response = await _httpClient.PostAsync("api/TheLoai/" + id, null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { mess = 1 });
                }
                else
                {
                    return RedirectToAction("Index", new { mess = 2 });
                }
            }
            else
            {
                return RedirectToAction("Index", new { mess = 2 });
            }

        }
    }
}
