using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface ITacGiaService
    {
        Task<IEnumerable<TacGia>> GetTacGiaAsync();
        Task<TacGia?> GetTacGiaAsync(string id);
        Task AddTacGiaAsync(TacGia par);
        Task UpdateTacGiaAsync(TacGia par);
        Task DeleteTacGiaAsync(string id);
    }
}
