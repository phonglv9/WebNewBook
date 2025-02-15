﻿    using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
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
            List<HomeViewModel> modelHome = new List<HomeViewModel>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/HomeVM").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelHome = JsonConvert.DeserializeObject<List<HomeViewModel>>(jsonData);


            };
        
            
            //danh muc
            List<DanhMucSach> danhMucSaches = new List<DanhMucSach>();
            HttpResponseMessage responseDM =  _httpClient.GetAsync(_httpClient.BaseAddress + "/home/DanhMuc").Result;
            if (responseDM.IsSuccessStatusCode)
            {
                string jsonData = responseDM.Content.ReadAsStringAsync().Result;
                danhMucSaches = JsonConvert.DeserializeObject<List<DanhMucSach>>(jsonData);

                ViewBag.DanhMuc = await danhMucSaches.ToListAsync();
            };
            //Sách mới
            ViewBag.NewBook = modelHome.OrderByDescending(c => c.NgayTao).Take(10).ToList();
            //Sách bán chạy
            List<ProductOderTop10VM> productOderTop10VMs = new List<ProductOderTop10VM>();
            HttpResponseMessage responseProductTop10 = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/HomeProductTop10Oder").Result;
            if (responseProductTop10.IsSuccessStatusCode)
            {
                string jsonData = responseProductTop10.Content.ReadAsStringAsync().Result;
                productOderTop10VMs = JsonConvert.DeserializeObject<List<ProductOderTop10VM>>(jsonData);

                ViewBag.SachBanChay = productOderTop10VMs;
            };



            return View(modelHome);
        }
        public async Task<IActionResult> Product(string search, string currentFilter, string iddanhmuc, string idtheloai, string idtacgia, string sortOrder, int? pageNumber, int pageSize, double priceMin, double priceMax)
        {

            #region Product
            //SanPham

            List<ProductVM> productStore = new List<ProductVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/Product").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                productStore = JsonConvert.DeserializeObject<List<ProductVM>>(jsonData);
            };

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
                    productStore = await productStore.OrderByDescending(c => c.GiaBan).ToListAsync();
                    break;
                case "Giá thấp nhất":
                    productStore = await productStore.OrderBy(c => c.GiaBan).ToListAsync();
                    break;
                case "A-Z":
                    productStore = await productStore.OrderBy(c => c.TenSanPham).ToListAsync();
                    break;
                case "Z-A":
                    productStore = await productStore.OrderByDescending(c => c.TenSanPham).ToListAsync();
                    break;

            }
            #region Fillter price
            if (priceMax != 0 || priceMin != 0)
            {
                productStore = await productStore.Where(c => c.GiaBan >= priceMin && c.GiaBan <= priceMax).ToListAsync();
                ViewBag.NumberProduct = productStore.Count();

            }

            #endregion



          
            #region Tìm kiếm           
            if (!String.IsNullOrEmpty(search) && !(iddanhmuc == "Tất cả sách"))
            {
                search = search.ToLower();
                productStore = await productStore.Where(c => c.idDanhMuc == iddanhmuc && c.TenSanPham.ToLower().Contains(search)).ToListAsync();
                if (productStore.Count == 0)
                {
                    ViewBag.TextSearch = $"Không tìm thấy kết quả {search} trong danh mục";
                }
                ViewBag.NumberProduct = productStore.Count();
                return View(await PaginatedList<ProductVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }

            if (!String.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                productStore = await productStore.Where(c => c.TenSanPham.ToLower().Contains(search)).ToListAsync();

                if (productStore.Count == 0 || productStore == null)
                {
                    ViewBag.TextSearch = "Không tìm thấy kết quả " + search;
                }

                ViewBag.NumberProduct = productStore.Count();

                return View(await PaginatedList<ProductVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }

            if (!String.IsNullOrEmpty(iddanhmuc))
            {
                if (iddanhmuc == "Tất cả sách")
                {
                    ViewBag.NumberProduct = productStore.Count();
                    return View(await PaginatedList<ProductVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));
                }
                productStore = await productStore.Where(c => c.idDanhMuc == iddanhmuc).ToListAsync();
                if (productStore == null || productStore.Count == 0)
                {
                    ViewBag.ProductNull = "Không có sản phẩm";

                }
                else
                {
                    ViewBag.ProductSS = productStore.Select(c => c.TenDanhMuc).Distinct();
                }
                ViewBag.NumberProduct = productStore.Count();

                return View(await PaginatedList<ProductVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));

            }
            if (!String.IsNullOrEmpty(idtheloai))
            {

                productStore = await productStore.Where(c => c.idTheLoai == idtheloai).ToListAsync();
                if (productStore == null || productStore.Count == 0)
                {
                    ViewBag.ProductNull = "Không có sản phẩm";

                }
                else
                {
                    ViewBag.ProductSS = productStore.Select(c => c.TenTheLoai).Distinct();
                }
                ViewBag.NumberProduct = productStore.Count();
                return View(await PaginatedList<ProductVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));


            }
            if (!String.IsNullOrEmpty(idtacgia))
            {

                productStore = await productStore.Where(c => c.idTacGia == idtacgia).ToListAsync();
                if (productStore == null || productStore.Count == 0)
                {
                    ViewBag.ProductNull = "Không có sản phẩm";

                }
                else
                {
                    ViewBag.ProductSS = productStore.Select(c => c.TenTacGia).Distinct();
                }
                ViewBag.NumberProduct = productStore.Count();
                return View(await PaginatedList<ProductVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));


            }


            #endregion


            ViewBag.NumberProduct = productStore.Count();
            return View(await PaginatedList<ProductVM>.CreateAsync(await productStore.ToListAsync(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> ProductDetaill(string id)
        {

            //Model home
            SanPhamChiTiet Product = new SanPhamChiTiet();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/home/ProductDetail/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                Product = JsonConvert.DeserializeObject<SanPhamChiTiet>(jsonData);


            };
            ViewBag.SanphamCT = Product;
            ViewBag.TheLoaiSP = Product.TheLoaiSP;
            ViewBag.SoLuongSach = Product.SoLuongSach;
            //Thể loại
            List<SanPhamChiTiet> lstchitetsp = new List<SanPhamChiTiet>();
            HttpResponseMessage responseTL = _httpClient.GetAsync(_httpClient.BaseAddress + $"/home/TheLoaict/{id}").Result;
            if (responseTL.IsSuccessStatusCode)
            {
                string jsonData = responseTL.Content.ReadAsStringAsync().Result;
                lstchitetsp = JsonConvert.DeserializeObject<List<SanPhamChiTiet>>(jsonData);
            };
            var checkBook = lstchitetsp.Where(c => c.TenSach != null).Select(c=>c.taiban).ToList();
            ViewBag.CheckBook = checkBook.Count();
            var gouptl = lstchitetsp.GroupBy(c => c.TenTheLoai).ToList();
            List<string> tl=new List<string>();
            if (gouptl != null)
            {
                foreach (var b in gouptl)
                {
                    foreach (var c in b.Take(1))
                    {
                        tl.Add(c.TenTheLoai);

                    }
                }
            }
            ViewBag.TheLoai = tl;

            var goupMT = lstchitetsp.GroupBy(c => c.Mota).ToList();
            List<SanPhamChiTiet> MT = new List<SanPhamChiTiet>();
            List<string> TS = new List<string>();
            if (gouptl != null)
            {
                foreach (var b in goupMT)
                {
                    foreach (var c in b.Take(1))
                    {
                        SanPhamChiTiet spct = new SanPhamChiTiet();
                        spct.TenSach = c.TenSach;
                        spct.Mota = c.Mota;
                        MT.Add(spct);
                    }
                }
            }
           
            ViewBag.Mota = MT;

			//Nhà xuất bản
			var goupNxb = lstchitetsp.GroupBy(c => c.TenNhaXuatBan).ToList();
			List<string> nxb = new List<string>();
			if (goupNxb != null)
			{
				foreach (var b in goupNxb)
				{
					foreach (var c in b.Take(1))
					{
						nxb.Add(c.TenNhaXuatBan);

					}
				}
			}
			ViewBag.NXB = nxb;

			//DanhMuc
			List<DanhMucSach> danhMucSaches = new List<DanhMucSach>();
            HttpResponseMessage responseDMm = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/DanhMuc").Result;
            if (responseDMm.IsSuccessStatusCode)
            {
                string jsonData = responseDMm.Content.ReadAsStringAsync().Result;
                danhMucSaches = JsonConvert.DeserializeObject<List<DanhMucSach>>(jsonData);

                
                   
                
                    ViewBag.DanhMuc = danhMucSaches.ToList();

                
            };
            // list all product
            List<ProductVM> modelProductVM = new List<ProductVM>();
            HttpResponseMessage responseDM = _httpClient.GetAsync(_httpClient.BaseAddress + "/home/Product").Result;
            if (responseDM.IsSuccessStatusCode)
            {
                string jsonData = responseDM.Content.ReadAsStringAsync().Result;
                modelProductVM = JsonConvert.DeserializeObject<List<ProductVM>>(jsonData);


            };
            var sp = modelProductVM.Where(a => a.ID_SanPham == id).FirstOrDefault();
               var listDM= modelProductVM.Where(b=>b.idDanhMuc==sp.idDanhMuc && b.ID_SanPham != id).ToList();
            
            ViewBag.listDM = listDM;





            
            var gouptg= lstchitetsp.GroupBy(c => c.TenTacGia).ToList();
            List<string> tg = new List<string>();
            if (gouptg != null) {
                foreach (var b in gouptg)
                {
                    foreach (var c in b.Take(1))
                    {
                        tg.Add(c.TenTacGia);

                    }
                }
            }

            ViewBag.TacGia = tg;
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