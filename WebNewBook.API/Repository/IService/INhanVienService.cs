using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface INhanVienService
    {
        Task<IEnumerable<NhanVien>> GetNhanVienAsync();
        Task<NhanVien?> GetNhanVienAsync(string id);
        Task AddNhanVienAsync(NhanVien par);
        Task UpdateNhanVienAsync(NhanVien par);
        Task DeleteNhanVienAsync(string id);
    }
}