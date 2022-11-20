using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IDanhMucService
    {
        Task AddDanhMucAsync(DanhMucSach danhMucSach);
        Task DeleteDanhMucAsync(string id);
        Task<IEnumerable<DanhMucSach>> GetDM();
        Task GetNhaXuatBanAsync(string id);
        Task UpdateDanhMucAsync(DanhMucSach danhMucSach);
    }
}
