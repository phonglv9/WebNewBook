using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;

namespace WebNewBook.API.Repository.Service
{
    public class VoucherService : IVoucherService
    {
        private readonly dbcontext _dbcontext;

        public VoucherService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddVouCherAsync(Voucher voucher)
        {
            string a;
            Random random = new Random();
            voucher.Id = "MPHVC" + random.Next().ToString();
            voucher.Createdate = DateTime.Now;
      
            voucher.MaNhanVien = "NV01";       
           voucher.TrangThai = 1;
            _dbcontext.Add(voucher);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteVouCherAsync(string id)
        {
            Voucher? voucher = _dbcontext.Vouchers.FirstOrDefault(c => c.Id == id) ?? null;
            if (voucher != null)
            {
                voucher.TrangThai = 0;
                 _dbcontext.Vouchers.Update(voucher);
                await _dbcontext.SaveChangesAsync();
            }
        }

  
        public async Task<IEnumerable<Voucher>> GetVouCherAsync()
        {
            return await _dbcontext.Vouchers.ToListAsync();
        }


        public async Task<Voucher?> GetVouCherByIdAsync(string id)
        {
            return await _dbcontext.Vouchers.FirstOrDefaultAsync(c => c.Id == id) ?? null;
        }

        public async Task UpdateVouCherAsync(Voucher voucher)
        {
            _dbcontext.Update(voucher);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
