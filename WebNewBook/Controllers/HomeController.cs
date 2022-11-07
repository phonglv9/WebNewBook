using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Models;
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

        public async Task<IActionResult> Product(string search, string currentFilter, string iddanhmuc, string idtheloai, string idtacgia, string sortOrder, int? pageNumber, int pageSize, double priceMin, double priceMax)
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
            ViewData["IdTheLoai"] = idtheloai;
            ViewData["IdTacGia"] = idtacgia;




            if (pageSize == 0)
            {
                pageSize = 20;
            }

            if (search != null)
            {
                pageNumber = 1;
            }
            else
            {
                search = currentFilter;
            }
            if (priceMin > priceMax)
            {
                priceMax = priceMin;
            }
            ViewBag.PriceMax = priceMax;
            ViewBag.PriceMin = priceMin;
            ViewData["PageNumber"] = pageNumber;
            ViewBag.PageSize = pageSize;
            switch (sortOrder)
            {
                case "Bán chạy nhất":

                    break;
                case "Giá cao nhất":
                    productStore = await productStore.OrderByDescending(c => c.sanPhams.GiaBan).ToListAsync();
                    break;
                case "Giá thấp nhất":
                    productStore = await productStore.OrderBy(c => c.sanPhams.GiaBan).ToListAsync();
                    break;
                case "A-Z":
                    productStore = await productStore.OrderBy(c => c.sanPhams.TenSanPham).ToListAsync();
                    break;
                case "Z-A":
                    productStore = await productStore.OrderByDescending(c => c.sanPhams.TenSanPham).ToListAsync();
                    break;

            }
            #region Fillter price
            if (priceMax != 0 || priceMin != 0)
            {
                productStore = await productStore.Where(c => c.sanPhams.GiaBan >= priceMin && c.sanPhams.GiaBan <= priceMax).ToListAsync();
                ViewBag.NumberProduct = productStore.Count();

            }

            #endregion




            #region Tìm kiếm           
            if (!String.IsNullOrEmpty(search) && !(iddanhmuc == "Tất cả sách"))
            {

                productStore = await productStore.Where(c => c.danhMucSach.ID_DanhMuc == iddanhmuc && c.sanPhams.TenSanPham.Contains(search)).ToListAsync();
                if (productStore.Count == 0)
                {
                    ViewBag.TextSearch = $"Không tìm thấy kết quả {search} trong danh mục";
                }
                ViewBag.NumberProduct = productStore.Count();
                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }

            if (!String.IsNullOrEmpty(search))
            {
                productStore = await productStore.Where(c => c.sanPhams.TenSanPham.Contains(search)).ToListAsync();

                if (productStore.Count == 0 || productStore == null)
                {
                    ViewBag.TextSearch = "Không tìm thấy kết quả " + search;
                }

                ViewBag.NumberProduct = productStore.Count();

                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }

            if (!String.IsNullOrEmpty(iddanhmuc))
            {
                if (iddanhmuc == "Tất cả sách")
                {
                    ViewBag.NumberProduct = productStore.Count();
                    return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));
                }
                productStore = await productStore.Where(c => c.danhMucSach.ID_DanhMuc == iddanhmuc).ToListAsync();
                if (productStore == null || productStore.Count == 0)
                {
                    ViewBag.ProductNull = "Không có sản phẩm";

                }
                else
                {
                    ViewBag.ProductSS = productStore.Select(c => c.danhMucSach.TenDanhMuc).Distinct();
                }
                ViewBag.NumberProduct = productStore.Count();

                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }
            if (!String.IsNullOrEmpty(idtheloai))
            {

                productStore = await productStore.Where(c => c.theLoai.ID_TheLoai == idtheloai).ToListAsync();
                if (productStore == null || productStore.Count == 0)
                {
                    ViewBag.ProductNull = "Không có sản phẩm";

                }
                else
                {
                    ViewBag.ProductSS = productStore.Select(c => c.theLoai.TenTL).Distinct();
                }
                ViewBag.NumberProduct = productStore.Count();
                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));


            }
            if (!String.IsNullOrEmpty(idtacgia))
            {

                productStore = await productStore.Where(c => c.tacGia.ID_TacGia == idtacgia).ToListAsync();
                if (productStore == null || productStore.Count == 0)
                {
                    ViewBag.ProductNull = "Không có sản phẩm";

                }
                else
                {
                    ViewBag.ProductSS = productStore.Select(c => c.tacGia.HoVaTen).Distinct();
                }
                ViewBag.NumberProduct = productStore.Count();
                return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));


            }


            #endregion


            ViewBag.NumberProduct = productStore.Count();
            return View(await PaginatedList<HomeVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> ProductDetaill(string id)
        {
            //Model home
            SanPhamChiTiet modelHome = new SanPhamChiTiet();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/home/ProductDetail/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelHome = JsonConvert.DeserializeObject<SanPhamChiTiet>(jsonData);


            };
            ViewBag.test = modelHome;

            //Thể loại
            List<TheLoai> theLoais = new List<TheLoai>();
            HttpResponseMessage responseTL = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/TheLoai").Result;
            if (responseTL.IsSuccessStatusCode)
            {
                string jsonData = responseTL.Content.ReadAsStringAsync().Result;
                theLoais = JsonConvert.DeserializeObject<List<TheLoai>>(jsonData);


            };
            if (!String.IsNullOrEmpty(modelHome.sachCT.MaTheLoai))
            {
                foreach (var a in modelHome.sachCT.MaTheLoai)
                {
                    List<TheLoai> lsttheLoai = new List<TheLoai>();
                    lsttheLoai = theLoais.Where(c => c.ID_TheLoai == a.ToString()).ToList();
                    ViewBag.TheLoai = lsttheLoai;
                }
            }





            //TÁC GIẢ
            List<TacGia> tacGias = new List<TacGia>();
            HttpResponseMessage responseTG = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/Tacgia").Result;
            if (responseTL.IsSuccessStatusCode)
            {
                string jsonData = responseTG.Content.ReadAsStringAsync().Result;
                tacGias = JsonConvert.DeserializeObject<List<TacGia>>(jsonData);


            };
            List<TacGia> LstTacGia = new List<TacGia>();



            LstTacGia = tacGias.Where(c => c.ID_TacGia == modelHome.sachCT.TacGia.ID_TacGia).ToList();

            ViewBag.TacGia = LstTacGia;


            return View("ProductDetail");
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