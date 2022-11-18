using Microsoft.EntityFrameworkCore;
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

        public async Task AddGioHangAsync(GioHang par)
        {
            _dbContext.Add(par);
            await _dbContext.SaveChangesAsync();
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
	}
}
