using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface ITheLoaiService
    {
        Task<IEnumerable<TheLoai>> GetTheLoaiAsync();
        Task<TheLoai?> GetTheLoaiAsync(string id);
        Task AddTheLoaiAsync(TheLoai par);
        Task UpdateTheLoaiAsync(TheLoai par);
        Task DeleteTheLoaiAsync(string id);
    }
}
