using WebNewBook.API.ModelsAPI;

namespace WebNewBook.API.Repository.IService
{
    public interface IReportService
    {
        //Task<List<ReportVM>> GetReportVMs();
        List<ReportDTO> GetReportNewBook(int type);
        List<ReportDTO> GetFillterReport(string startDate, string endDate);
        List<ReportProductDTO> GetReportProduct();
        List<ReportProductDTO> GetReportProductTop10();
    }
}
