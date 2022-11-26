using Microsoft.EntityFrameworkCore;
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
                throw null;
            }
        }
        public async Task AddHoaDonCT(List<HoaDonCT> hoaDonCTs)
        {
            if (hoaDonCTs != null)
            {

               
                dbcontext.HoaDonCTs.AddRange(hoaDonCTs);
                dbcontext.SaveChanges();
            }
            else
            {
                throw null;
            }



        }
        public async Task UpdateSLSanPham(List<HoaDonCT> hoaDonCTs)
        {
            if (hoaDonCTs != null)
            {

                foreach (var item in hoaDonCTs)
                {
                    var sanPham =  await dbcontext.SanPhams.Where(c => c.ID_SanPham == item.MaSanPham).FirstOrDefaultAsync();
                    if (sanPham != null)
                        sanPham.SoLuong = sanPham.SoLuong - item.SoLuong;
                     dbcontext.SanPhams.UpdateRange(sanPham);
                    await  dbcontext.SaveChangesAsync();

                }
               
            }
            else
            {
                throw null;
            }



        }
        public async Task UpdateSLSanPhamVNPay(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

              var hoaDonCTs = dbcontext.HoaDonCTs.Where(c => c.MaHoaDon == id).ToList();
                foreach (var item in hoaDonCTs)
                {
                    var sanPham = await dbcontext.SanPhams.Where(c => c.ID_SanPham == item.MaSanPham).FirstOrDefaultAsync();
                    if (sanPham != null)
                        sanPham.SoLuong = sanPham.SoLuong - item.SoLuong;
                    dbcontext.SanPhams.UpdateRange(sanPham);
                    await dbcontext.SaveChangesAsync();

                }

            }
            else
            {
                throw null;
            }



        }
        public async Task UpdateTrangThai(string id)
        {
            var hoaDon = dbcontext.HoaDons.Where(c => c.ID_HoaDon == id).FirstOrDefault();
            if (hoaDon != null)
            {
                hoaDon.TrangThai = 2;
                dbcontext.HoaDons.Update(hoaDon);
                await dbcontext.SaveChangesAsync();

            }
            else
            {
                throw null;
            }
        }

        public async Task<HoaDon?> GetHoaDon(string id)
        {
            try
            {
                return await dbcontext.HoaDons.FirstOrDefaultAsync(c => c.ID_HoaDon == id) ?? null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<ViewHoaDon>> GetListHoaDon()
        {
            var listkhachhang = dbcontext.KhachHangs.ToList();
            var listhoadon = dbcontext.HoaDons.ToList();
            var listhoadonct = dbcontext.HoaDonCTs.ToList();
            var listsanpham = dbcontext.SanPhams.ToList();
            var listsanphamct = dbcontext.SanPhamCTs.ToList();


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
            return viewhd;
        }

        public async Task<ViewHoaDonCT> GetListid(string id)
        {
            var listhoadon = dbcontext.HoaDons.ToList();
            var listkhachhang = dbcontext.KhachHangs.ToList();
            var listhoadonct = dbcontext.HoaDonCTs.ToList();
            var listsanpham = dbcontext.SanPhams.ToList();
            var listsanphamct = dbcontext.SanPhamCTs.ToList();
            var listsach = dbcontext.Sachs.ToList();
            var listsachct = dbcontext.SachCTs.ToList();
            var listtheloai = dbcontext.TheLoais.ToList();
            var listtacgia = dbcontext.TacGias.ToList();
            var listnhaxuatban = dbcontext.NhaXuatBans.ToList();
            var listphieunhap = dbcontext.PhieuNhaps.ToList();
            var listnhanvien = dbcontext.NhanViens.ToList();

            var viewhd = (/*from a in listhoadon*/
                          //join b in  listkhachhang on a.MaKhachHang equals b.ID_KhachHang
                          from c in listhoadonct
                          join d in listsanpham on c.MaSanPham equals d.ID_SanPham
                          join f in listsanphamct on d.ID_SanPham equals f.MaSanPham
                          join g in listsach on f.MaSach equals g.ID_Sach
                          join h in listsachct on g.ID_Sach equals h.MaSach
                          join j in listtheloai on h.MaTheLoai equals j.ID_TheLoai
                          join k in listtacgia on h.MaTacGia equals k.ID_TacGia
                          join l in listnhaxuatban on g.MaNXB equals l.ID_NXB
                          join z in listphieunhap on g.ID_Sach equals z.MaSach
                          join x in listnhanvien on z.MaNhanVien equals x.ID_NhanVien
                          select new ViewHoaDonCT()
                          {
                              //hoaDon = a,
                              //KhachHang = b,

                              hoaDonCT = c,
                              sanPham = d,
                              sanPhamCT = f,
                              sach = g,
                              sachCT = h,
                              theLoai = j,
                              tacGia = k,
                              nhaXuatBan = l,
                              phieuNhap = z,
                              nhanVien = x,
                          }


                         ).ToList();
            var HoaDonChiTiet = viewhd.Where(a => a.hoaDonCT.ID_HDCT == id).FirstOrDefault();


            return HoaDonChiTiet;
        }

        public async  Task<HoaDon?> Updatetrangthai(string id,int name)
        {
            var a = dbcontext.HoaDons.Where(a=>a.ID_HoaDon==id).FirstOrDefault();
            a.TrangThai = name;
            dbcontext.Update(a);
            await dbcontext.SaveChangesAsync();
            return a;

        }
    }
}
