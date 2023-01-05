
using WebNewBook.Model;

namespace WebNewBook.API.ModelsAPI
{
    public class ViewHoaDon
    {
        public KhachHang KhachHang { get; set; } = new KhachHang();
        public HoaDon hoaDon { get; set; } = new HoaDon();
        public HoaDonCT hoaDonCT { get; set; } = new HoaDonCT();
        public SanPham sanPham { get; set; } = new SanPham();
        public SanPhamCT sanPhamCT { get; set; } = new SanPhamCT();
       
    }
    public class ViewHoaDonCT
    {
        public KhachHang KhachHang { get; set; } = new KhachHang();
        public HoaDon hoaDon { get; set; } = new HoaDon();
        public HoaDonCT hoaDonCT { get; set; } = new HoaDonCT();
        public SanPham sanPham { get; set; } = new SanPham();
        public SanPhamCT sanPhamCT { get; set; } = new SanPhamCT();
        public SachCT sachCT { get; set; } = new SachCT();
        public NhaXuatBan nhaXuatBan { get; set; } = new NhaXuatBan();
       
       
    }
}
