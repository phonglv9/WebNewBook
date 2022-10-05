using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;
namespace WebNewBook.API.Repository.Service
{
    public class Voucher : IVoucher
    {
        private readonly dbcontext _dbcontext;

        public Voucher(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddVouCherAsync(VoucherCT phieuGiamGia)
        {
            phieuGiamGia.Id = Guid.NewGuid().ToString();

            _dbcontext.Add(phieuGiamGia);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteVouCherAsync(string id)
        {
            VoucherCT? phieuGiamGia = _dbcontext.VoucherCTs.FirstOrDefault(c => c.Id == id) ?? null;
            if (phieuGiamGia != null)
            {
               // phieuGiamGia.TrangThai = 0;
                _dbcontext.VoucherCTs.Update(phieuGiamGia);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<VoucherCT?> GetVouCherByIdAsync(string id)
        {
            return await _dbcontext.VoucherCTs.FirstOrDefaultAsync(c => c.Id == id) ?? null;
        }


  
        public async Task<IEnumerable<VoucherCT>> GetVouCherAsync()
        {
               var model = await _dbcontext.VoucherCTs.ToListAsync();
               return model;
        }

        public async Task UpdateVouCherAsync(VoucherCT phieuGiamGia)
        {
            _dbcontext.Update(phieuGiamGia);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
