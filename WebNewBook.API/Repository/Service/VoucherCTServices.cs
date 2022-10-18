using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class VoucherCTServices : IVoucherCTServices
    {
        private readonly dbcontext _dbcontext;

        public VoucherCTServices(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddAutomaticallyAsync(VoucherCT voucherCT)
        {
            voucherCT.TrangThai = 0;
            voucherCT.CreateDate = DateTime.Now;
            voucherCT.NgayHetHan = voucherCT.Voucher.EndDate;
            _dbcontext.Add(voucherCT);
            await _dbcontext.SaveChangesAsync();
        }

        public Task AddImportExcerAsync(VoucherCT voucherCT)
        {
            throw new NotImplementedException();
        }

        public Task AddManuallyAsync(VoucherCT voucherCT)
        {
            throw new NotImplementedException();
        }

        public Task<VoucherCT?> GetVoucherByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VoucherCT>> GetVoucherChuaphathanhAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VoucherCT>> GetVoucherDaphathanhAsync()
        {
            throw new NotImplementedException();
        }

        public Task HuyVouCherAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task PhathanhVouCherAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
