using WebNewBook.Model;
namespace WebNewBook.API.Repository.IService
{
    public interface IProfileCustomerService
    {
        Task<List<HoaDon>> GetOrdersAsync(string makhachhang);
        Task<List<HoaDonCT>> GetOrderDetailAsync(string mahoadon);

    }
}
