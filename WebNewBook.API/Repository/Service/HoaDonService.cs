using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
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
            
                
                dbcontext.HoaDons.Add(hoaDon);
                dbcontext.SaveChanges();
            
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
            var hoaDon = dbcontext.HoaDons.FirstOrDefault(c => c.ID_HoaDon == id);
            var customer = dbcontext.KhachHangs.FirstOrDefault(c => c.ID_KhachHang == hoaDon.MaKhachHang);
            if (hoaDon != null && customer != null)
            {
                hoaDon.TrangThai = 2;
               customer.DiemTichLuy = customer.DiemTichLuy + Convert.ToInt32(hoaDon.TongTien) / 100;
              dbcontext.KhachHangs.Update(customer);
                  
                
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
            //var listhoadonct = dbcontext.HoaDonCTs.ToList();
            //var listsanpham = dbcontext.SanPhams.ToList();
            //var listsanphamct = dbcontext.SanPhamCTs.ToList();


            var viewhd = (from a in listkhachhang
                          join b in listhoadon on a.ID_KhachHang equals b.MaKhachHang
                          //join c in listhoadonct on b.ID_HoaDon equals c.MaHoaDon
                          //join d in listsanpham on c.MaSanPham equals d.ID_SanPham
                          //join f in listsanphamct on d.ID_SanPham equals f.MaSanPham

                          select new ViewHoaDon()
                          {
                              KhachHang = a,
                              hoaDon = b,
                             // hoaDonCT = c,
                              //sanPham = d,
                              //sanPhamCT = f,

                          }
                         ).ToList();
            var listGoupBy = viewhd.OrderBy(c => c.hoaDon.TrangThai).ToList();
            return listGoupBy;
        }

        public async Task<List<ViewHoaDonCT>> GetHDCT(string id)
        {
            var listhoadon = dbcontext.HoaDons.ToList();
            var listkhachhang = dbcontext.KhachHangs.ToList();
            var listhoadonct = dbcontext.HoaDonCTs.ToList();
            var listsanpham = dbcontext.SanPhams.ToList();
            var listsanphamct = dbcontext.SanPhamCTs.ToList();
            var listsach = dbcontext.Sachs.ToList();
            //var listsachct = dbcontext.SachCTs.ToList();
           /// var listtheloai = dbcontext.TheLoais.ToList();
            //var listtacgia = dbcontext.TacGias.ToList();
            var listnhaxuatban = dbcontext.NhaXuatBans.ToList();
            //var listphieunhap = dbcontext.PhieuNhaps.ToList();
            //var listnhanvien = dbcontext.NhanViens.ToList();

            var viewhdCT = ( from a in listkhachhang join  b in listhoadon on a.ID_KhachHang equals b.MaKhachHang
                            join c in listhoadonct on b.ID_HoaDon equals c.MaHoaDon
                             join q in listsanpham on c.MaSanPham equals q.ID_SanPham
                            join w in listsanphamct on q.ID_SanPham equals w.MaSanPham
                            join e in listsach on w.MaSach equals e.ID_Sach
                            //join r in listsachct on e.ID_Sach equals r.MaSach
                            join h in listnhaxuatban on e.MaNXB equals h.ID_NXB

                            select new ViewHoaDonCT()
                            {
                                KhachHang = a,
                                hoaDon = b,
                                hoaDonCT = c,
                                sanPham = q,
                                sanPhamCT = w,
                                sach=e,
                                nhaXuatBan = h,
                                
                            }

                         ).ToList();
            var HoaDonChiTiet = viewhdCT.Where(a => a.hoaDonCT.MaHoaDon == id).ToList();
           //var listhdct = viewhdCT.GroupBy(c => c.hoaDonCT.MaHoaDon==id).ToList();
            
            
            return HoaDonChiTiet;
        }

        public async  Task UpdatetrangthaiHD(string id,int name)
        {
            

            try
            {
                var a = dbcontext.HoaDons.Where(a => a.ID_HoaDon == id).FirstOrDefault();
                a.TrangThai = name;
                dbcontext.Update(a);
                 await dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
