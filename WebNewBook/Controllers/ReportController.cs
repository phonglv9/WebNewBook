using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebNewBook.API.ModelsAPI;
using WebNewBook.ViewModel;
using X.PagedList;

namespace WebNewBook.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        Uri baseAdress = new Uri("https://localhost:7266/api");
        HttpClient _httpClient;
        public ReportController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAdress;
        }
        public JsonResult GetReportNewBook(int type)
        {
            List<ReportDTO> modelReport = new List<ReportDTO>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/Report/NewBook/{type}").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelReport = JsonConvert.DeserializeObject<List<ReportDTO>>(jsonData);

            }

            return Json(modelReport, new System.Text.Json.JsonSerializerOptions());
        }
        public JsonResult GetFillterReport(string startDate, string endDate)
        {
            List<ReportDTO> modelReport = new List<ReportDTO>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/Report/Fillter?startDate="+startDate+"&endDate="+endDate+"").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelReport = JsonConvert.DeserializeObject<List<ReportDTO>>(jsonData);

            }

            return Json(modelReport, new System.Text.Json.JsonSerializerOptions());
        }
        public JsonResult GetReportProduct()
        {
            List<ReportProductDTO> modelReport = new List<ReportProductDTO>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/Report/Product").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelReport = JsonConvert.DeserializeObject<List<ReportProductDTO>>(jsonData);

            }

            return Json(modelReport, new System.Text.Json.JsonSerializerOptions());
        }
        public JsonResult GetReportProductTop10()
        {
            List<ReportProductDTO> modelReport = new List<ReportProductDTO>();
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"/Report/ProductTop10").Result;
            if (response.IsSuccessStatusCode)
            {
                string jsonData = response.Content.ReadAsStringAsync().Result;
                modelReport = JsonConvert.DeserializeObject<List<ReportProductDTO>>(jsonData);

            }

            return Json(modelReport, new System.Text.Json.JsonSerializerOptions());
        }
        public  IActionResult Index()
        {
            ViewBag.TitleAdmin = "DashBoard";
            return View();
        }
        //public async Task< IActionResult> Index(string nxb, string fillterDatetime, DateTime timerangefrom, DateTime rimerangeto)
        //{   

        //    //ListReport
        //    List<ReportVM> modelReport = new List<ReportVM>();
        //    HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Report").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string jsonData = response.Content.ReadAsStringAsync().Result;
        //        modelReport = JsonConvert.DeserializeObject<List<ReportVM>>(jsonData);


        //    }   
        //    if (fillterDatetime == "month")
        //    {
        //        modelReport = await modelReport.Where(c => c.hoaDon.NgayMua.Month == DateTime.Now.Month).ToListAsync();
        //        DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //        DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        //        var dataChart = Enumerable.Range(0, 1 + (end - start).Days).Select(x => start.AddDays(x))
        //        .GroupJoin(modelReport, dt => dt, o => o.hoaDon.NgayMua.Date, (dt, orders) => new ReportDTO { DateValue = dt.ToString("ddMMyyyy"), TotalMoney = orders.Sum(c => c.hoaDon.TongTien) }).ToList();
        //        ViewBag.ChartData = dataChart;
        //        ViewBag.MessChart = fillterDatetime;
        //        return View(modelReport);
        //    }
        //    if (fillterDatetime == "year")
        //    {

        //        //ListReportYear
        //        List<ReportDTO> modelReportYear = new List<ReportDTO>();
        //        HttpResponseMessage responseyear = _httpClient.GetAsync(_httpClient.BaseAddress + "/Report/GetYear").Result;
        //        if (responseyear.IsSuccessStatusCode)
        //        {
        //            string jsonData = response.Content.ReadAsStringAsync().Result;
        //            modelReportYear = JsonConvert.DeserializeObject<List<ReportDTO>>(jsonData);

        //            ViewBag.ChartData = modelReportYear;
        //        }

        //        ViewBag.MessChart = fillterDatetime;
        //        return View(modelReport);
        //    }
        //    if (timerangefrom.Year != 1 && rimerangeto.Year != 1)
        //    {
        //        modelReport = modelReport.Where(x => x.hoaDon.NgayMua >= timerangefrom && x.hoaDon.NgayMua <= rimerangeto).ToList();
        //        var dataChart = Enumerable.Range(0, 1 + (rimerangeto - timerangefrom).Days).Select(x => timerangefrom.AddDays(x))
        //        .GroupJoin(modelReport, dt => dt, o => o.hoaDon.NgayMua.Date, (dt, orders) => new ReportDTO { DateValue = dt.ToString(), TotalMoney = orders.Sum(c => c.hoaDon.TongTien) }).ToList();
        //        ViewBag.ChartData = dataChart;
        //        ViewBag.MessChart = "FillterX";
        //        return View(modelReport);                         
        //    }
        //    return View(modelReport);


        //}


    }
}
