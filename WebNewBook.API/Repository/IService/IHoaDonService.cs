using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IHoaDonService
    {
        Task AddHoaDon(HoaDon hoaDon);
        Task AddHoaDonCT(List<HoaDonCT> hoaDonCTs);
        Task UpdateTrangThai(string id);
        Task<HoaDon?> GetHoaDon(string id);
        Task UpdateSLSanPham(List<HoaDonCT> hoaDonCTs);
        Task UpdateSLSanPhamVNPay(string id);
        Task<List<ViewHoaDon>> GetListHoaDon();
        Task<ViewHoaDonCT> GetListid(string id);
    }
}
