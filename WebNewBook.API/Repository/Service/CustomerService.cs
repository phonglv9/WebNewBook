using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly dbcontext _dbcontext;

        public CustomerService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddKhachHangAsync(KhachHang khachHang)
        {
            try
            {
                if (khachHang.ID_KhachHang == null)
                {
                    khachHang.ID_KhachHang = Guid.NewGuid().ToString();
                }

                khachHang.TrangThai = 1;
                _dbcontext.Add(khachHang);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task DeleteKhachHangAsync(string id)
        {
            try
            {
                KhachHang? khachhang = _dbcontext.KhachHangs.FirstOrDefault(c => c.ID_KhachHang == id) ?? null;
                if (khachhang != null)
                {
                    if (khachhang.TrangThai == 1)
                    {
                        khachhang.TrangThai = 0;
                        _dbcontext.KhachHangs.Update(khachhang);
                        await _dbcontext.SaveChangesAsync();
                    }
                    else
                    {
                        khachhang.TrangThai = 1;
                        _dbcontext.KhachHangs.Update(khachhang);
                        await _dbcontext.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<KhachHang?> GetKhachHangByIdAsync(string id)
        {
            try
            {
                return await _dbcontext.KhachHangs.FirstOrDefaultAsync(c => c.ID_KhachHang == id) ?? null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<KhachHang>> GetKhachHangsAsync(string search, int? status)
        {
            try
            {
                var model = await _dbcontext.KhachHangs.Where(c => (status != null ? c.TrangThai == status : true) && ((!string.IsNullOrEmpty(search) ? c.HoVaTen.ToLower().Contains(search) : true) || (!string.IsNullOrEmpty(search) ? c.Email.ToLower().Contains(search) : true) || (!string.IsNullOrEmpty(search) ? c.SDT.ToLower().Contains(search) : true)
                                                                                    )).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task UpdateKhachHangAsync(KhachHang khachHang)
        {
            try
            {
                _dbcontext.Update(khachHang);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<KhachHang?> GetKhachHangByEmail(string email)
        {
            try
            {
                return await _dbcontext.KhachHangs.FirstOrDefaultAsync(c => c.Email == email) ?? null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
