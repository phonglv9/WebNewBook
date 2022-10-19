using Microsoft.EntityFrameworkCore;
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
         
            throw new NotImplementedException();
        }

        public Task AddImportExcerAsync(VoucherCT voucherCT)
        {
            throw new NotImplementedException();
        }

        public async Task AddManuallyAsync(VoucherCT voucherCT)
        {
            voucherCT.NgayBatDau = null;
            voucherCT.TrangThai = 0;
            voucherCT.CreateDate = DateTime.Now;

            _dbcontext.Add(voucherCT);
            await _dbcontext.SaveChangesAsync();
        }

        public Task<VoucherCT?> GetVoucherByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VoucherCT?>> GetVoucherByMaVoucherAsync(string id)
        {
            return await _dbcontext.VoucherCTs.Where(c => c.MaVoucher == id).ToListAsync();
        }

        public async Task<IEnumerable<VoucherCT>> GetVoucherChuaphathanhAsync()
        {
            return await _dbcontext.VoucherCTs.Where(c=>c.TrangThai==0).ToListAsync();
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
