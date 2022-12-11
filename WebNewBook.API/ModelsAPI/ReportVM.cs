using WebNewBook.Model;

namespace WebNewBook.API.ModelsAPI
{
    public class ReportVM
    {
        public KhachHang khachHang { get; set; }
        public HoaDon hoaDon { get; set; }
        public HoaDonCT hoaDonCT { get; set; }
        public SanPham sanPham { get; set; }
    }
}
