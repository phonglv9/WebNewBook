using WebNewBook.Model;
namespace WebNewBook.API.Repository.IService
{
    public interface IVoucherService
    {
        Task<IEnumerable<Voucher>> GetVouCherAsync();
        Task<Voucher?> GetVouCherByIdAsync(string id);
        Task AddVouCherAsync(Voucher voucher);
        Task UpdateVouCherAsync(Voucher voucher);
        Task DeleteVouCherAsync(string id);
       
    }
}
