using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class HoaDonService: IHoaDonService
    {
        private readonly dbcontext dbcontext;

        public HoaDonService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task AddHoaDon(HoaDon hoaDon )
        {
            if (hoaDon != null )
            {
                dbcontext.HoaDons.Add(hoaDon);
                dbcontext.SaveChanges();
            }
            else
            {
                throw new Exception("404");
            }
        }
        public async Task AddHoaDonCT( List<HoaDonCT> hoaDonCTs)
        {
            if (hoaDonCTs != null)
            {
              
                foreach (var item in hoaDonCTs)
                {
                    var x = dbcontext.SanPhams.Where(c => c.ID_SanPham == item.MaSanPham).FirstOrDefault();
                    x.SoLuong = x.SoLuong - item.SoLuong;
                    dbcontext.SanPhams.Update(x);

                }

                dbcontext.HoaDonCTs.AddRange(hoaDonCTs);
                dbcontext.SaveChanges();
            }
            else
            {
                throw new Exception("404");
            }
        }

    }
}
