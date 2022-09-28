using WebNewBook.Model;
namespace WebNewBook.API.Repository.IService
{
    public interface ICustomerService
    {
        Task<IEnumerable<KhachHang>> GetKhachHangsAsync();
        Task<KhachHang?> GetKhachHangByIdAsync(string id);
        Task AddKhachHangAsync(KhachHang khachHang);
        Task UpdateKhachHangAsync(KhachHang khachHang);
        Task DeleteKhachHangAsync(string id);
    }
}
