using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        IReportService _reportService;
        public ReportController(IReportService reportService) {
            _reportService = reportService;
        }
        [HttpGet("NewBook/{type}")]
        public IEnumerable<ReportDTO> GetReport(int type)
        {

         return  _reportService.GetReportNewBook(type);

        }
        [HttpGet("Fillter")]
        public IEnumerable<ReportDTO> GetFillterReport(string startDate, string endDate)
        {

            return _reportService.GetFillterReport(startDate,endDate);

        }
        [HttpGet("Product")]
        public IEnumerable<ReportProductDTO> GetReportProduct()
        {

            return _reportService.GetReportProduct();

        }
        [HttpGet("ProductTop10")]
        public IEnumerable<ReportProductDTO> GetReportProductTOP10()
        {

            return _reportService.GetReportProductTop10();

        }


    }
}
