using WebNewBook.Model;
namespace WebNewBook.API.Repository.IService
{
    public interface IVoucherCTServices
    {
        Task<IEnumerable<VoucherCT>> GetVoucherOfCustomer(string maCustomer);
        Task<IEnumerable<VoucherCT>> GetVoucherChuaphathanhAsync();
        Task<IEnumerable<VoucherCT>> GetVoucherDaphathanhAsync();
        Task<VoucherCT?> GetVoucherByIdAsync(string id);
        Task<IEnumerable<VoucherCT?>> GetVoucherByMaVoucherAsync(string id);
        Task AddManuallyAsync(VoucherCT voucherCT);
        Task AddAutomaticallyAsync(int quantityVoucher, int sizeVoucher, string startTextVoucher, string endTextVoucher,string maVoucher);
        Task AddImportExcerAsync(IFormFile file, string Phathanh);
        Task PhathanhVouCherAsync(string id);
        Task HuyVouCherAsync(List<string> id);
    }
}
