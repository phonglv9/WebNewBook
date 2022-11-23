using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IPhieuGiamSPService
    {
        Task<IEnumerable<PhieuGiamGiaSP>> GetPhieuGiamGiaSPAsync();
        Task<PhieuGiamGiaSP?> GetPhieuGiamGiaSPAsync(string id);
        Task AddPhieuGiamGiaSPAsync(PhieuGiamGiaSP par);
        Task UpdatePhieuGiamGiaSPAsync(PhieuGiamGiaSP par);
        Task DeletePhieuGiamGiaSPAsync(string id);
    }
}
