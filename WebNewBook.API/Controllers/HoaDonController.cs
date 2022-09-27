using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Data;

using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        public dbcontext _db;

        public HoaDonController(dbcontext dbcontext)
        {
            _db = dbcontext;
        }
        [HttpGet]
        public async Task<List<ViewHoadon>> getlist()
        {
           
            var listkhachhang = _db.KhachHangs.ToList();
            var listhoadon = _db.HoaDons.ToList();
            var listhoadonct = _db.HoaDonCTs.ToList();
            var listsanpham = _db.SanPhams.ToList();
            var listsanphamct = _db.SanPhamCTs.ToList();
            var listsach = _db.Sachs.ToList();
            var listsachct = _db.SachCTs.ToList();
            var listtheloai = _db.TheLoais.ToList();
            var listtacgia = _db.TacGias.ToList();
            var listnhaxuatban = _db.NhaXuatBans.ToList();
            var listphieunhap = _db.PhieuNhaps.ToList();
            var listnhanvien = _db.NhanViens.ToList();

            var viewhd = (from a in listkhachhang
                          join b in listhoadon on a.ID_KhachHang equals b.MaKhachHang
                          join c in listhoadonct on b.ID_HoaDon equals c.MaHoaDon
                          join d in listsanpham on c.MaSanPham equals d.ID_SanPham
                          join f in listsanphamct on d.ID_SanPham equals f.MaSanPham
                          join g in listsach on f.MaSach equals g.ID_Sach
                          join h in listsachct on g.ID_Sach equals h.MaSach
                          join j in listtheloai on h.MaTheLoai equals j.ID_TheLoai
                          join k in listtacgia on h.MaTacGia equals k.ID_TacGia
                          join l in listnhaxuatban on g.MaNXB equals l.ID_NXB
                          join z in listphieunhap on g.ID_Sach equals z.MaSach
                          join x in listnhanvien on z.MaNhanVien equals x.ID_NhanVien
                          select new ViewHoadon()
                          {
                              KhachHang=a,
                              hoaDon=b,
                              hoaDonCT=c,
                              sanPham=d,
                              sanPhamCT=f,
                              sach=g,
                              sachCT=h,
                              theLoai=j,
                              tacGia=k,
                              nhaXuatBan=l,
                              phieuNhap=z,
                              nhanVien=x,
                          }


                         ).ToList();


            return viewhd;
        }
    }
}
