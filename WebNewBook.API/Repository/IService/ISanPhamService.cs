using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface ISanPhamService
    {
        Task<IEnumerable<SanPham>> GetSanPhamAsync();
        Task<SanPham?> GetSanPhamAsync(string id);
        Task AddSanPhamAsync(SanPham par, IEnumerable<string> Sachs);
        Task UpdateSanPhamAsync(SanPham par);
        Task<List<Sach>?> GetSachsBySanPhamAsync(string id);
    }
}
