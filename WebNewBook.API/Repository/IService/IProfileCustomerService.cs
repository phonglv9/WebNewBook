using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
namespace WebNewBook.API.Repository.IService
{
    public interface IProfileCustomerService
    {
        Task<List<HoaDon>> GetOrdersAsync(string makhachhang);
        Task<List<HoaDonCT>> GetOrderDetailAsync(string mahoadon);
        Task<HoaDon?> GetOrderByIdAsync(string id);
        Task<List<ViewHoaDon>> GetListOrder(string mahoadon);
        Task HuyOrder(string id);
    }
}
