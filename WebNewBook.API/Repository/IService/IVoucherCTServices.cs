using WebNewBook.Model;
namespace WebNewBook.API.Repository.IService
{
    public interface IVoucherCTServices
    {
        Task<IEnumerable<VoucherCT>> GetVoucherChuaphathanhAsync();
        Task<IEnumerable<VoucherCT>> GetVoucherDaphathanhAsync();
        Task<VoucherCT?> GetVoucherByIdAsync(string id);
        Task AddManuallyAsync(VoucherCT voucherCT);
        Task AddAutomaticallyAsync(VoucherCT voucherCT);
        Task AddImportExcerAsync(VoucherCT voucherCT);
        Task PhathanhVouCherAsync(string id);
        Task HuyVouCherAsync(string id);
    }
}
