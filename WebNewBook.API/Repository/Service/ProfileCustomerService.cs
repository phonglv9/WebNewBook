using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
namespace WebNewBook.API.Repository.Service
{
    public class ProfileCustomerService : IProfileCustomerService
    {
        private readonly dbcontext _dbcontext;

        public ProfileCustomerService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<HoaDon?> GetOrderByIdAsync(string id)
        {
            try
            {
                return await _dbcontext.HoaDons.FirstOrDefaultAsync(c => c.ID_HoaDon == id) ?? null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<HoaDonCT>> GetOrderDetailAsync(string mahoadon)
        {
            try
            {
             var model=  await _dbcontext.HoaDonCTs.Where(c => c.MaHoaDon == mahoadon).ToListAsync();
                return model;
            }
            catch (Exception)
            {

                throw;
            };
        }

        public async Task<List<HoaDon>> GetOrdersAsync(string makhachhang)
        {
            try
            {
                var model = await _dbcontext.HoaDons.Where(c => c.MaKhachHang == makhachhang ).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<ViewHoaDon>> GetListOrder(string mahoadon)
        {
            var listkhachhang = _dbcontext.KhachHangs.ToList();
            var listhoadon = _dbcontext.HoaDons.ToList();
            var listhoadonct = _dbcontext.HoaDonCTs.ToList();
            var listsanpham = _dbcontext.SanPhams.ToList();
            var listsanphamct = _dbcontext.SanPhamCTs.ToList();


            var viewhd = (from a in listkhachhang
                          join b in listhoadon on a.ID_KhachHang equals b.MaKhachHang
                          join c in listhoadonct on b.ID_HoaDon equals c.MaHoaDon
                          join d in listsanpham on c.MaSanPham equals d.ID_SanPham
                          join f in listsanphamct on d.ID_SanPham equals f.MaSanPham

                          select new ViewHoaDon()
                          {
                              KhachHang = a,
                              hoaDon = b,
                              hoaDonCT = c,
                              sanPham = d,
                              sanPhamCT = f,

                          }
                         ).ToList();

            return  viewhd.Where(c => c.hoaDon.ID_HoaDon == mahoadon).ToList(); ;
        }
    }
}
