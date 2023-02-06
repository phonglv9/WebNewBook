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
    public class QLNhaXuatBanController : Controller
    {
        private readonly HttpClient _httpClient;

        public QLNhaXuatBanController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7266/");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
        }
        private async Task<List<NhaXuatBan>?> GetNXB()
        {
            List<NhaXuatBan> nhaXuatBans = new List<NhaXuatBan>();
            HttpResponseMessage responseGet = await _httpClient.GetAsync("api/NhaXuatBan");
            if (responseGet.IsSuccessStatusCode)
            {
                string jsonData = responseGet.Content.ReadAsStringAsync().Result;
                nhaXuatBans = JsonConvert.DeserializeObject<List<NhaXuatBan>>(jsonData);
            };
            return nhaXuatBans;
        }
       
        public async Task<IActionResult> Index(string? timKiem, int ?trangThai, int?page,string mess)
        {
            ViewBag.TitleAdmin = "Nhà Xuất Bản";
            ViewBag.TimKiem = timKiem;
            ViewBag.TrangThai = trangThai;
            ViewBag.message = mess;
           var pageNumber = page ?? 1;
            List<NhaXuatBan>? nhaXuatBans = new List<NhaXuatBan>();
            nhaXuatBans = await GetNXB();
            if (!string.IsNullOrEmpty(timKiem))
            {
                timKiem = timKiem.ToLower();
               
                 nhaXuatBans = nhaXuatBans.Where(c => c.TenXuatBan.ToLower().Contains(timKiem)).ToList();
               
            }
            if (trangThai != 0)
            {         
            switch (trangThai)
            {
                case 1:
                    nhaXuatBans = nhaXuatBans.Where(c => c.TrangThai == 1).ToList();

                        break;
                       
                       
                case 2:
                    nhaXuatBans = nhaXuatBans.Where(c => c.TrangThai == 0).ToList();
                        break;
                        
                default:
                        nhaXuatBans = nhaXuatBans.ToList();
                        break;
            }
            }
            ViewBag.NXB = nhaXuatBans.ToPagedList(pageNumber, 5);
            return View();
            
        }
        public async Task<IActionResult> AddNBX(NhaXuatBan nhaXuatBan)
        {
            nhaXuatBan.ID_NXB = "NXB" + DateTime.Now.Ticks;
            nhaXuatBan.TrangThai = 1;
            if (nhaXuatBan != null)
            {
                
                    StringContent content = new StringContent(JsonConvert.SerializeObject(nhaXuatBan), Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync("api/NhaXuatBan", content);
                    
                       
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
        public async Task<IActionResult> UpdateNXB(NhaXuatBan nhaXuatBan)
        {
            if (nhaXuatBan != null)
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(nhaXuatBan), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("api/NhaXuatBan", content);


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
        public async Task<IActionResult> RemoveNXB(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                HttpResponseMessage response = await _httpClient.PostAsync("api/NhaXuatBan/" + id, null);
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
