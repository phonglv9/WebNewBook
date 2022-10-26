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
            try
            {
                string a;
                Random random = new Random();
                voucher.Id = "MPHVC" + random.Next().ToString();
                voucher.SoLuong = 0;
                voucher.Createdate = DateTime.Now;

                voucher.MaNhanVien = "NV01";
                voucher.TrangThai = 1;
                _dbcontext.Add(voucher);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task DeleteVouCherAsync(string id)
        {
            try
            {
                Voucher? voucher = _dbcontext.Vouchers.FirstOrDefault(c => c.Id == id) ?? null;
                if (voucher != null)
                {
                    voucher.TrangThai = 0;
                    _dbcontext.Vouchers.Update(voucher);
                    await _dbcontext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

  
        public async Task<IEnumerable<Voucher>> GetVouCherAsync()
        {
            try
            {
                return await _dbcontext.Vouchers.ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<Voucher?> GetVouCherByIdAsync(string id)
        {
            try
            {
                return await _dbcontext.Vouchers.FirstOrDefaultAsync(c => c.Id == id) ?? null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task UpdateVouCherAsync(Voucher voucher)
        {
            try
            {
                _dbcontext.Update(voucher);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
