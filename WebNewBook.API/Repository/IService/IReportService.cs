using WebNewBook.API.ModelsAPI;

namespace WebNewBook.API.Repository.IService
{
    public interface IReportService
    {
        Task<List<ReportVM>> GetReportVMs();
    }
}
