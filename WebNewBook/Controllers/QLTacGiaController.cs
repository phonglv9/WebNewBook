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
    public class QLTacGiaController : Controller
    {
        private readonly HttpClient _httpClient;

        public QLTacGiaController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }
        private async Task<List<TacGia>?> GetTG()
        {
            List<TacGia> tacGias = new List<TacGia>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("api/TacGia");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                tacGias = JsonConvert.DeserializeObject<List<TacGia>>(jsonData);
            };
            return tacGias;
        }

        public async Task<IActionResult> Index(string? timKiem, int? trangThai, int? page, string mess)
        {
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
            var pageNumber = page ?? 1;
            List<TacGia>? tacGias = new List<TacGia>();
            tacGias = await GetTG();
            if (!string.IsNullOrEmpty(timKiem))
            {
                timKiem = timKiem.ToLower();

                tacGias = tacGias.Where(c => c.HoVaTen.ToLower().Contains(timKiem)).ToList();

            }
            if (trangThai != 0)
            {
                switch (trangThai)
                {
                    case 1:
                        tacGias = tacGias.Where(c => c.TrangThai == 1).ToList();

                        break;


                    case 2:
                        tacGias = tacGias.Where(c => c.TrangThai == 0).ToList();
                        break;

                    default:
                        tacGias = tacGias.ToList();
                        break;
                }
            }
            ViewBag.TG = tacGias.ToPagedList(pageNumber, 15);
            return View();

        }
        public async Task<IActionResult> AddTG(TacGia tacGia)
        {
            tacGia.ID_TacGia = "NXB" + DateTime.Now.Ticks;
            tacGia.TrangThai = 1;
            if (tacGia != null)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(tacGia), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/TacGia", content);


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
        public async Task<IActionResult> UpdateTG(TacGia tacGia)
        {
            if (tacGia != null)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(tacGia), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/TacGia", content);


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
        public async Task<IActionResult> RemoveTG(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                HttpResponseMessage response = await _httpClient.PostAsync("api/TacGia/" + id, null);
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

