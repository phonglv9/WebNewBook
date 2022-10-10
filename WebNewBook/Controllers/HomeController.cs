using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Models;
using WebNewBook.ViewModel;
using X.PagedList;

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
                
    
            };
            //TheLoai
            List<TheLoai> theLoais = new List<TheLoai>();
            HttpResponseMessage responseTL = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/TheLoai").Result;
            if (responseTL.IsSuccessStatusCode)
            {
                string jsonData = responseTL.Content.ReadAsStringAsync().Result;
                theLoais = JsonConvert.DeserializeObject<List<TheLoai>>(jsonData);

                ViewBag.TheLoai = theLoais.ToList();
            };
            //danh muc
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
        public async Task<IActionResult> Product(string search, string currentFilter,string iddanhmuc,string idtheloai, string idtacgia, string sortOrder, int? pageNumber,double priceMin , double priceMax)
            {

            #region Product

            
            //DanhMuc
            List<DanhMucSach> danhMucSaches = new List<DanhMucSach>();
            HttpResponseMessage responseDM = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/DanhMuc").Result;
            if (responseDM.IsSuccessStatusCode)
            {
                string jsonData = responseDM.Content.ReadAsStringAsync().Result;
                danhMucSaches = JsonConvert.DeserializeObject<List<DanhMucSach>>(jsonData);

                ViewBag.DanhMuc = await danhMucSaches.ToListAsync();
            };
            //TheLoai
            List<TheLoai> theLoais = new List<TheLoai>();
            HttpResponseMessage responseTL = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/TheLoai").Result;
            if (responseTL.IsSuccessStatusCode)
            {
                string jsonData = responseTL.Content.ReadAsStringAsync().Result;
                theLoais = JsonConvert.DeserializeObject<List<TheLoai>>(jsonData);

                ViewBag.TheLoai = await theLoais.ToListAsync();
            };
            //TacGia
            List<TacGia> tacGias = new List<TacGia>();
            HttpResponseMessage responseTG = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/TacGia").Result;
            if (responseTL.IsSuccessStatusCode)
            {
                string jsonData = responseTG.Content.ReadAsStringAsync().Result;
                tacGias = JsonConvert.DeserializeObject<List<TacGia>>(jsonData);

                ViewBag.TacGia = await tacGias.ToListAsync();
            };

            //SanPham

            List<HomeVM> productStore = new List<HomeVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/Product").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                productStore = JsonConvert.DeserializeObject<List<HomeVM>>(jsonData);
            };

           


            #endregion

            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = search;
            ViewData["IdDanhMuc"] = iddanhmuc;
       
            int pageSize = 3;
            if (search != null)
            {
                pageNumber = 1;
            }
            else
            {
                search = currentFilter;
            }
         

            #region Tìm kiếm           
            if (!String.IsNullOrEmpty(search) && !(iddanhmuc == "Tất cả sách"))
            {
                productStore = productStore.Where(c => c.danhMucSach.ID_DanhMuc == iddanhmuc && c.sanPhams.TenSanPham.Contains(search)).ToList();
                if (productStore.Count == 0)
                {
                    ViewBag.TextSearch = $"Không tìm thấy kết quả {search} trong danh mục";
                }
                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }

            if (!String.IsNullOrEmpty(search))
            {
                productStore = await productStore.Where(c => c.sanPhams.TenSanPham.Contains(search)).ToListAsync();
                if (productStore.Count == 0)
                {
                    ViewBag.TextSearch = "Không tìm thấy kết quả " + search;
                }
                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }

            if (!String.IsNullOrEmpty(iddanhmuc))
            {
                if (iddanhmuc =="Tất cả sách")
                {
                    return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));
                }
                productStore = await productStore.Where(c => c.danhMucSach.ID_DanhMuc == iddanhmuc).ToListAsync();
                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }
            if (!String.IsNullOrEmpty(idtheloai))
            {
             
                productStore = await productStore.Where(c => c.theLoai.ID_TheLoai == idtheloai).ToListAsync();
                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }
            if (!String.IsNullOrEmpty(idtacgia))
            {

                productStore = await productStore.Where(c => c.tacGia.ID_TacGia == idtacgia).ToListAsync();
                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }

            switch (sortOrder)
            {
                //case "nameDesc":
                //    students = students.OrderByDescending(s => s.LastName);
                //    break;
                //case "date":
                //    students = students.OrderBy(s => s.EnrollmentDate);
                //    break;
                //case "dateDesc":
                //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    break;
                //default:
                //    students = students.OrderBy(s => s.LastName);
                //    break;
            }
            #endregion

            #region FillterOderby

            #endregion


            return View(await PaginatedList<HomeVM>.CreateAsync( await productStore.ToListAsync(), pageNumber ?? 1, pageSize));
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