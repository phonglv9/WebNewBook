using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class HoaDonCTService : IHoaDonCTService
    {
        private readonly dbcontext _dbcontext;

        public HoaDonCTService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddOrderDetail(string mhd, string masp)
        {

            var giabansp = _dbcontext.SanPhams.FirstOrDefault(c => c.ID_SanPham == masp).GiaBan;
            HoaDonCT hoaDonCT = new HoaDonCT
            {
                ID_HDCT = "hdct" + Guid.NewGuid().ToString(),
                MaSanPham = masp,
                MaHoaDon = mhd,
                SoLuong = 1,
                GiaBan = giabansp
            };
            _dbcontext.HoaDonCTs.Add(hoaDonCT);
            await _dbcontext.SaveChangesAsync();

        }

        public async Task DeletaOrderDetail(string mhdct)
        {
            var modal = _dbcontext.HoaDonCTs.FirstOrDefault(c => c.ID_HDCT == mhdct);
            _dbcontext.HoaDonCTs.Remove(modal);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateOrderDetailQuantity(string mhdct, int quantity)
        {
            var modal = _dbcontext.HoaDonCTs.FirstOrDefault(c => c.ID_HDCT == mhdct );
            modal.SoLuong = quantity;
            _dbcontext.HoaDonCTs.Update(modal);
             await _dbcontext.SaveChangesAsync();
        }
    }
}
