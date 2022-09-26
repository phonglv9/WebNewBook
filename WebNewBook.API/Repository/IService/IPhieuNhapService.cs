using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IPhieuNhapService
    {
        Task<IEnumerable<PhieuNhap>> GetPhieuNhapAsync();
        Task<PhieuNhap?> GetPhieuNhapAsync(string id);
        Task AddPhieuNhapAsync(PhieuNhap par);
        Task UpdatePhieuNhapAsync(PhieuNhap par);
        Task DeletePhieuNhapAsync(string id);
    }
}
