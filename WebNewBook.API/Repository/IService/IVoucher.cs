using WebNewBook.Model;
namespace WebNewBook.API.Repository.IService
{
    public interface IVoucher
    {
        Task<IEnumerable<VoucherCT>> GetVouCherAsync();
        Task<VoucherCT?> GetVouCherByIdAsync(string id);
        Task AddVouCherAsync(VoucherCT phieuGiamGia);
        Task UpdateVouCherAsync(VoucherCT phieuGiamGia);
        Task DeleteVouCherAsync(string id);
    }
}
