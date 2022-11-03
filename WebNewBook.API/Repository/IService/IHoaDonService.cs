using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IHoaDonService
    {
        Task AddHoaDon(HoaDon hoaDon);
        Task AddHoaDonCT(List<HoaDonCT> hoaDonCTs);
    }
}
