using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
              
                dynamic checkTrung;
                
                do
                {
                    Random random = new Random();
                    string id = "MPHVC" + random.Next().ToString();
                     checkTrung = _dbcontext.Vouchers.Where(c => c.Id == id).FirstOrDefault();
                    voucher.Id = id;
                } while (checkTrung!=null);



               
                voucher.SoLuong = 0;
                voucher.Createdate = DateTime.Now;
           //     voucher.MaNhanVien = "NV01";
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
                var dateNow = DateTime.Now;
                return await _dbcontext.Vouchers.Where(c => c.TrangThai == 1 &&   dateNow <= c.EndDate.AddDays(1)).ToListAsync();
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
                if (voucher!=null)
                {
                    var model= _dbcontext.Vouchers.FirstOrDefault(c=>c.Id == voucher.Id);
                    if (model.SoLuong==0)
                    {
                        model.Createdate = DateTime.Now;
                        model.StartDate=voucher.StartDate;
                        model.EndDate=voucher.EndDate;
                        model.GhiChu= voucher.GhiChu;
                        model.MenhGiaDieuKien = voucher.MenhGiaDieuKien;
                        model.MenhGia = model.MenhGia;
                        model.TenPhatHanh = voucher.TenPhatHanh;
                        model.HinhThuc=  voucher.HinhThuc;
                        model.DiemDoi = voucher.DiemDoi;
                        _dbcontext.Update(model);
                        await _dbcontext.SaveChangesAsync();
                    }
                    else
                    {
                        model.Createdate = DateTime.Now;
                        model.TenPhatHanh = voucher.TenPhatHanh;
                        model.GhiChu=voucher.GhiChu;    
                        _dbcontext.Update(model);
                        await _dbcontext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
