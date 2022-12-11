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
        [HttpGet]
        public async Task< IEnumerable<ReportVM>> GetReport()
        {
         return await _reportService.GetReportVMs();

        }
    }
}
