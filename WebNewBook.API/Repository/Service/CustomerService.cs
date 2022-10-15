using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;

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
            khachHang.ID_KhachHang = Guid.NewGuid().ToString();
            khachHang.TrangThai = 1;
            _dbcontext.Add(khachHang);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteKhachHangAsync(string id)
        {
            KhachHang? khachhang = _dbcontext.KhachHangs.FirstOrDefault(c => c.ID_KhachHang == id) ?? null;
            if (khachhang != null)
            {
                if (khachhang.TrangThai==1)
                {
                    khachhang.TrangThai = 0;
                    _dbcontext.KhachHangs.Update(khachhang);
                    await _dbcontext.SaveChangesAsync();
                }
              
else                {
                    khachhang.TrangThai = 1;
                    _dbcontext.KhachHangs.Update(khachhang);
                    await _dbcontext.SaveChangesAsync();
                }
              
            }
        }

        public async Task<KhachHang?> GetKhachHangByIdAsync(string id)
        {
            return await _dbcontext.KhachHangs.FirstOrDefaultAsync(c => c.ID_KhachHang == id) ?? null;
        }

        public async Task<IEnumerable<KhachHang>> GetKhachHangsAsync(string search , int? status)
        {
            var model= await _dbcontext.KhachHangs.Where(c =>(status !=null ? c.TrangThai==status:true)&& ((!string.IsNullOrEmpty(search)? c.HoVaTen.ToLower().Contains(search) :true) || (!string.IsNullOrEmpty(search) ? c.Email.ToLower().Contains(search) : true) || (!string.IsNullOrEmpty(search) ? c.SDT.ToLower().Contains(search) : true)
                                                                              )).ToListAsync();
            
            return model;
        }

        public async Task UpdateKhachHangAsync(KhachHang khachHang)
        {
            _dbcontext.Update(khachHang);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
