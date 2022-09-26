using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Models;
using WebNewBook.ReadAPI;

namespace WebNewBook.Controllers
{
    public class HomeController : Controller
    {      
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;
        public HomeController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;


        }

        public async Task<IActionResult> Index()
        {
            List<HomeVM> modelHome = new List<HomeVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/HomeVM").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelHome = JsonConvert.DeserializeObject<List<HomeVM>>(jsonData);
                ViewBag.DanhMuc =  modelHome.Select(c => c.danhMucSach).ToList();
                ViewBag.TheLoai =  modelHome.Select(c => c.theLoai).ToList();
            };
            //Doc ra list doi tuong tu json 
            return View(modelHome);
        }
        public async Task< IActionResult> Product(string search, string iddanhmuc)
        {
            List<HomeVM> modelHome = new List<HomeVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/Product").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                 modelHome = JsonConvert.DeserializeObject<List<HomeVM>>(jsonData);
            };
            #region Tìm kiếm           
            if (!String.IsNullOrEmpty(search)  && !(iddanhmuc == "Tất cả sách"))
            {            
                    modelHome =  modelHome.Where(c => c.danhMucSach.ID_DanhMuc == iddanhmuc && c.sanPhams.TenSanPham.StartsWith(search)).ToList();
                    if (modelHome.Count == 0)
                    {
                    ViewBag.TextSearch = $"Không tìm thấy kết quả {search} trong danh mục"  ;
                    }
                    return View(modelHome);              
                
            }
           
            if (!String.IsNullOrEmpty(search))
              {
                    modelHome = modelHome.Where(c => c.sanPhams.TenSanPham.StartsWith(search)).ToList();
                    if (modelHome.Count == 0)
                    {
                        ViewBag.TextSearch = "Không tìm thấy kết quả " + search;
                    }
                    return View(modelHome);

              }
                      
            if (iddanhmuc != "Tất cả sách")
            {
                modelHome = modelHome.Where(c => c.danhMucSach.ID_DanhMuc == iddanhmuc).ToList();
                return View(modelHome);

            }
            #endregion

            #region FillterOderby

            #endregion


            return View(modelHome);
        }      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}