using WebNewBook.API.ModelsAPI;
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
        Task<List<ViewHoaDonCT>> GetHDCT(string id);
        Task UpdatetrangthaiHD(string id,int name, string lydohuy);
        Task SendMailOder(string idHoaDon);
        //voucher get price
        Task<Voucher> GetPriceVoucher(string idVoucherCT);
        Task UpdateThongtinnguoinhan(HoaDon hoaDon);
        string AddOrderAdmin(HoaDon hoaDon);


    }
}
