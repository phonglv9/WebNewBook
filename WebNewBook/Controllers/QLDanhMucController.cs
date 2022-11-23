using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WebNewBook.Model;
using X.PagedList;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QLDanhMucController : Controller
    {
        private readonly HttpClient _httpClient;

        public QLDanhMucController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }
        private async Task<List<DanhMucSach>?> GetDM()
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

        public async Task<IActionResult> Index(string? timKiem, int? trangThai, int? page, string mess)
        {
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            var pageNumber = page ?? 1;
            List<DanhMucSach>? danhMucSaches = new List<DanhMucSach>();
            danhMucSaches = await GetDM();
            if (!string.IsNullOrEmpty(timKiem))
            {
                timKiem = timKiem.ToLower();

                danhMucSaches = danhMucSaches.Where(c => c.TenDanhMuc.ToLower().Contains(timKiem)).ToList();

            }
            if (trangThai != 0)
            {
                switch (trangThai)
                {
                    case 1:
                        danhMucSaches = danhMucSaches.Where(c => c.TrangThai == 1).ToList();

                        break;


                    case 2:
                        danhMucSaches = danhMucSaches.Where(c => c.TrangThai == 0).ToList();
                        break;

                    default:
                        danhMucSaches = danhMucSaches.ToList();
                        break;
                }
            }
            ViewBag.DM = danhMucSaches.ToPagedList(pageNumber, 15);
            return View();

        }
        public async Task<IActionResult> AddDM(DanhMucSach danhMucSach)
        {
            danhMucSach.ID_DanhMuc = "DM" + DateTime.Now.Ticks;
            danhMucSach.TrangThai = 1;
            if (danhMucSach != null)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(danhMucSach), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/DanhMucSach", content);


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
        public async Task<IActionResult> UpdateDM(DanhMucSach danhMucSach)
        {
            if (danhMucSach != null)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(danhMucSach), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/DanhMucSach", content);


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
        public async Task<IActionResult> RemoveDM(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                HttpResponseMessage response = await _httpClient.PostAsync("api/DanhMucSach/" + id, null);
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
