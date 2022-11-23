using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
	public class GioHangService : IGioHangService
	{
        private readonly dbcontext _dbContext;
        public GioHangService(dbcontext dbContext)
        {
            

            _dbContext = dbContext;
        }

        public async Task<int> AddGioHangAsync(string HinhAnh, int SoLuongs, string emailKH, string idsp)
        {
            var listsp = _dbContext.SanPhams.ToList();
            var listgh = _dbContext.GioHangs.ToList();
            GioHang giohangs = new GioHang();
            giohangs.ID_GioHang= "GH" + Guid.NewGuid().ToString();
            giohangs.HinhAnh = HinhAnh;
            giohangs.SoLuong = 0;
            giohangs.emailKH = emailKH;
            giohangs.idsp = idsp;
            giohangs.TenSP = listsp.Where(o => o.ID_SanPham == idsp).Select(c => c.TenSanPham).FirstOrDefault();
            giohangs.DonGia= listsp.Where(o => o.ID_SanPham == idsp).Select(c => c.GiaBan).FirstOrDefault();
              if (listgh.Exists(c => c.idsp == idsp))
            {
                var giohang= listgh.SingleOrDefault(c=>c.idsp== idsp);
                if (SoLuongs!=null)
                {
                    
                    giohang.SoLuong+= SoLuongs;
                    _dbContext.Update(giohang);
                    
                    return await _dbContext.SaveChangesAsync();

                    
                }


            }
            else
            {
                giohangs.SoLuong += SoLuongs;
            }
            _dbContext.Add(giohangs);
            await _dbContext.SaveChangesAsync();
            return 1;
        }

        public async Task<List<GioHang>> GetlistGH()
        {
            var listgh= await _dbContext.GioHangs.ToListAsync();
            return listgh;
        }

        public async Task<SanPham> GetSanPham( string ID)
        { 
            
            var b =  _dbContext.SanPhams.SingleOrDefault(a=>a.ID_SanPham==ID);

            return  b ;
        }

        public async Task<List<HomeVM>> VM()
		{
            var sanPham = await _dbContext.SanPhams.ToListAsync();
            var sanPhamCT = await _dbContext.SanPhamCTs.ToListAsync();
            var sachCT = await _dbContext.SachCTs.ToListAsync();
            var sach = await _dbContext.Sachs.ToListAsync();
            var theLoai = await _dbContext.TheLoais.ToListAsync();
            var danhMuc = await _dbContext.DanhMucSachs.ToListAsync();
            var tacgia = await _dbContext.TacGias.ToListAsync();
            List<HomeVM> homeVMs = new List<HomeVM>();
            homeVMs = (from b in sanPhamCT
                       join c in sach on b.MaSach equals c.ID_Sach
                       join d in sachCT on c.ID_Sach equals d.MaSach
                       join e in theLoai on d.MaTheLoai equals e.ID_TheLoai
                       join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                       join g in tacgia on d.MaTacGia equals g.ID_TacGia
                       select new HomeVM()
                       {

                           SanPhamCT = b,
                           sach = c,
                           sachCT = d,
                           theLoai = e,
                           danhMucSach = f,
                           tacGia = g,
                       }).ToList();
            return homeVMs;
        }

        public async Task<string> XoakhoiGioHang(string id)
        {
            var listgh = _dbContext.GioHangs.ToList();
            var giohang = listgh.SingleOrDefault(c => c.idsp ==id );
            _dbContext.Remove(giohang);
            await _dbContext.SaveChangesAsync();
            return "xoa thành công";
        }
    }
}
