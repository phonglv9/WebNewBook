using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class HoaDonService : IHoaDonService
    {
        private readonly dbcontext dbcontext;

        public HoaDonService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task AddHoaDon(HoaDon hoaDon)
        {
            if (hoaDon != null)
            {
                dbcontext.HoaDons.Add(hoaDon);
                dbcontext.SaveChanges();
            }
            else
            {
                throw new Exception("404");
            }
        }
        public async Task AddHoaDonCT(List<HoaDonCT> hoaDonCTs)
        {
            if (hoaDonCTs != null)
            {

                foreach (var item in hoaDonCTs)
                {
                    var sanPham = dbcontext.SanPhams.Where(c => c.ID_SanPham == item.MaSanPham).FirstOrDefault();
                    if (sanPham != null)
                        sanPham.SoLuong = sanPham.SoLuong - item.SoLuong;
                    dbcontext.SanPhams.UpdateRange(sanPham);
                    dbcontext.SaveChanges();

                }
                dbcontext.HoaDonCTs.AddRange(hoaDonCTs);
                dbcontext.SaveChanges();
            }
            else
            {
                throw new Exception("404");
            }



        }
        public async Task UpdateTrangThai(string id)
        {
            var hoaDon = dbcontext.HoaDons.Where(c => c.ID_HoaDon == id).FirstOrDefault();
            if (hoaDon != null)
            {
                hoaDon.TrangThai = 1;
                dbcontext.HoaDons.Update(hoaDon);
                await dbcontext.SaveChangesAsync();

            }
            else
            {
                throw new Exception("404");
            }
        }
    }
}
