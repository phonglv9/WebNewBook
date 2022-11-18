using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface INhaXuatBanService
    {
        Task<IEnumerable<NhaXuatBan>> GetNhaXuatBanAsync();
        Task<NhaXuatBan?> GetNhaXuatBanAsync(string id);
        Task AddNhaXuatBanAsync(NhaXuatBan par);
        Task UpdateNhaXuatBanAsync(NhaXuatBan par);
        Task DeleteNhaXuatBanAsync(string id);
    }
}
