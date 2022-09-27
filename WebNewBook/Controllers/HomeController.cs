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
            //Model home
            List<HomeVM> modelHome = new List<HomeVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/HomeVM").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelHome = JsonConvert.DeserializeObject<List<HomeVM>>(jsonData);
                //ViewBag.DanhMuc = modelHome.Select(c => c.danhMucSach).ToList();
                //ViewBag.TheLoai = modelHome.Select(c => c.theLoai).ToList();
            };
            //TheLoai
            List<TheLoai> theLoais = new List<TheLoai>();
            HttpResponseMessage responseTL = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/TheLoai").Result;
            if (responseTL.IsSuccessStatusCode)
            {
                string jsonData = responseTL.Content.ReadAsStringAsync().Result;
                theLoais = JsonConvert.DeserializeObject<List<TheLoai>>(jsonData);
                //ViewBag.DanhMuc = modelHome.Select(c => c.danhMucSach).ToList();
                ViewBag.TheLoai = theLoais.ToList();
            };
            //DanhMuc
            List<DanhMucSach> danhMucSaches = new List<DanhMucSach>();
            HttpResponseMessage responseDM = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/DanhMuc").Result;
            if (responseDM.IsSuccessStatusCode)
            {
                string jsonData = responseDM.Content.ReadAsStringAsync().Result;
                danhMucSaches = JsonConvert.DeserializeObject<List<DanhMucSach>>(jsonData);
                ViewBag.DanhMuc = danhMucSaches.ToList();

            };

            return View(modelHome);
        }
        public async Task<IActionResult> Product(string search, string iddanhmuc)
        {

            //TheLoai
            List<TheLoai> theLoais = new List<TheLoai>();
            HttpResponseMessage responseTL = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/TheLoai").Result;
            if (responseTL.IsSuccessStatusCode)
            {
                string jsonData = responseTL.Content.ReadAsStringAsync().Result;
                theLoais = JsonConvert.DeserializeObject<List<TheLoai>>(jsonData);
                //ViewBag.DanhMuc = modelHome.Select(c => c.danhMucSach).ToList();
                ViewBag.TheLoai = theLoais.ToList();
            };
            //DanhMuc
            List<DanhMucSach> danhMucSaches = new List<DanhMucSach>();
            HttpResponseMessage responseDM = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/DanhMuc").Result;
            if (responseDM.IsSuccessStatusCode)
            {
                string jsonData = responseDM.Content.ReadAsStringAsync().Result;
                danhMucSaches = JsonConvert.DeserializeObject<List<DanhMucSach>>(jsonData);
                ViewBag.DanhMuc = danhMucSaches.ToList();

            };
            
            //SanPham

            List<HomeVM> productStore = new List<HomeVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/HomeProduct").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                productStore = JsonConvert.DeserializeObject<List<HomeVM>>(jsonData);
            };
            #region Tìm kiếm           
            if (!String.IsNullOrEmpty(search)  && !(iddanhmuc == "Tất cả sách"))
            {
                productStore = productStore.Where(c => c.danhMucSach.ID_DanhMuc == iddanhmuc && c.sanPhams.TenSanPham.StartsWith(search)).ToList();
                    if (productStore.Count == 0)
                    {
                       ViewBag.TextSearch = $"Không tìm thấy kết quả {search} trong danh mục"  ;
                    }
                    return View(productStore);              
                
            }
           
            if (!String.IsNullOrEmpty(search))
              {
                productStore = productStore.Where(c => c.sanPhams.TenSanPham.StartsWith(search)).ToList();
                    if (productStore.Count == 0)
                    {
                        ViewBag.TextSearch = "Không tìm thấy kết quả " + search;
                    }
                    return View(productStore);

              }
                      
            if (iddanhmuc != "Tất cả sách")
            {
                productStore = productStore.Where(c => c.danhMucSach.ID_DanhMuc == iddanhmuc).ToList();
                return View(productStore);

            }
            #endregion

            #region FillterOderby

            #endregion


            return View(productStore);
        }
        public async Task<IActionResult> ProductDetail()
        {
            return View();
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