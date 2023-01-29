using WebNewBook.Model;
using WebNewBook.Model.APIModels;

namespace WebNewBook.API.Repository.IService
{
    public interface ISanPhamService
    {
        Task<IEnumerable<SanPham>> GetSanPhamAsync();
        Task<SanPham?> GetSanPhamAsync(string id);
        Task AddSanPhamAsync(SanPhamAPI par);
        Task UpdateSanPhamAsync(SanPham par);
        Task<List<SachCTViewModel>?> GetSachsBySanPhamAsync(string id);
        IEnumerable<SanPhamViewModel> GetSanPhamViewModel();
    }
}
