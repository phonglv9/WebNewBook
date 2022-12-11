using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebNewBook.API.ModelsAPI;
using WebNewBook.ViewModel;
using X.PagedList;

namespace WebNewBook.Controllers
{
    public class ReportController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;
        public ReportController()
        {

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;
        }

        public async Task< IActionResult> Index(string nxb, string fillterDatetime, DateTime timerangefrom, DateTime rimerangeto)
        {   

            //ListReport
            List<ReportVM> modelReport = new List<ReportVM>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Report").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelReport = JsonConvert.DeserializeObject<List<ReportVM>>(jsonData);
                

            };

           
            //DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //var res = Enumerable
            //    .Range(0, 1 + (end - start).Days)
            //    .Select(x => start.AddDays(x))
            //    .GroupJoin(modelReport,
            //        dt => dt, o => o.hoaDon.NgayMua.Date,

            //        (dt, orders) => new OrderDateSummary { Date = dt, Total = orders.Sum(c => c.hoaDon.TongTien) }).ToList();
            

            //ViewBag.Bieudo = res;
     
            if (nxb != null)
            {
                modelReport = modelReport.Where(c => c.sanPham.TenSanPham.ToLower().Contains(nxb)).ToList();
                return View(modelReport);

            }
            if (fillterDatetime == "day")
            {

                modelReport = await modelReport.Where(c => c.hoaDon.NgayMua.Day == DateTime.Now.Day).ToListAsync();

                var hour = DateTime.Now.Hour;
                DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var dataChart = Enumerable.Range(0, 1 + hour).Select(c => start.AddHours(c))
                .GroupJoin(modelReport, dt => dt, o => o.hoaDon.NgayMua, (dt, orders) => new OrderDateSummary { Date = dt, Total = orders.Sum(c => c.hoaDon.TongTien) }).ToList();
                ViewBag.MessChart = fillterDatetime;
                ViewBag.ChartData = dataChart;
                return View(modelReport);
            }
            if (fillterDatetime == "month")
            {
                modelReport = await modelReport.Where(c => c.hoaDon.NgayMua.Month == DateTime.Now.Month).ToListAsync();
                DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var dataChart = Enumerable.Range(0, 1 + (end - start).Days).Select(x => start.AddDays(x))
                .GroupJoin(modelReport, dt => dt, o => o.hoaDon.NgayMua.Date, (dt, orders) => new OrderDateSummary { Date = dt, Total = orders.Sum(c => c.hoaDon.TongTien) }).ToList();
                ViewBag.ChartData = dataChart;
                 ViewBag.MessChart = fillterDatetime;
                return View(modelReport);
            }
            if (fillterDatetime == "year")
            {
              
                modelReport = modelReport.Where(c => c.hoaDon.NgayMua.Year == DateTime.Now.Year).ToList();
                var tongtien = modelReport.Sum(c => c.hoaDon.TongTien);
                DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                IEnumerable<OrderDateSummary> dataChart = new List<OrderDateSummary> { new OrderDateSummary { Date = DateTime.Now, Total = tongtien } }; 
          
                ViewBag.ChartData = dataChart;
                ViewBag.MessChart = fillterDatetime;
                return View(modelReport);
            }
            if (timerangefrom.Year != 1 && rimerangeto.Year != 1)
            {

                modelReport = modelReport.Where(x => x.hoaDon.NgayMua >= timerangefrom && x.hoaDon.NgayMua <= rimerangeto).ToList();
            
                return View(modelReport);
                
            }
            else
            {
              modelReport = await modelReport.Where(c => c.hoaDon.NgayMua.Day == DateTime.Now.Day).ToListAsync();

                var hour = DateTime.Now.Hour;
                DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                var dataChart = Enumerable.Range(0, 1 + hour).Select(c => start.AddHours(c))
                .GroupJoin(modelReport, dt => dt, o => o.hoaDon.NgayMua, (dt, orders) => new OrderDateSummary { Date = dt, Total = orders.Sum(c => c.hoaDon.TongTien) }).ToList();

                ViewBag.ChartData = dataChart;
                return View(modelReport);
            }


        }
      

    }
}
