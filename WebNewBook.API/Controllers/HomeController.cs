using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly dbcontext _dbContext;
        public HomeController(dbcontext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet("HomeVM")]
        public async Task<ActionResult<List<HomeVM>>> GetHomVM()
        {
            var sanPham = _dbContext.SanPhams.ToList();
            var sanPhamCT = _dbContext.SanPhamCTs.ToList();
            var sachCT = _dbContext.SachCTs.ToList();
            var sach = _dbContext.Sachs.ToList();
            var theLoai = _dbContext.TheLoais.ToList();
            var danhMuc = _dbContext.DanhMucSachs.ToList();
            List<HomeVM> homeVMs = new List<HomeVM>();
            homeVMs = (from a in sanPham
                      join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham     
                      join c in sachCT on b.MaSachCT equals c.ID_SachCT
                      join d in sach on c.MaSach equals d.ID_Sach
                      join e in theLoai on c.MaTheLoai equals e.ID_TheLoai
                      join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                      select new HomeVM()
                      {
                          sanPhams = a,
                          SanPhamCT = b,
                          sachCT = c,
                          sach = d,
                          theLoai =e,
                          danhMucSach = f,
                      }).ToList();
            
            return Ok(homeVMs);
              
        }
        [HttpGet("Product")]
        public async Task<ActionResult<List<HomeVM>>> GetProduct()
        {
            var sanPham = _dbContext.SanPhams.ToList();
            var sanPhamCT = _dbContext.SanPhamCTs.ToList();
            var sachCT = _dbContext.SachCTs.ToList();
            var sach = _dbContext.Sachs.ToList();
            var theLoai = _dbContext.TheLoais.ToList();
            var danhMuc = _dbContext.DanhMucSachs.ToList();
            List<HomeVM> homeVMs = new List<HomeVM>();
            homeVMs = (from a in sanPham
                       join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham
                       join c in sachCT on b.MaSachCT equals c.ID_SachCT
                       join d in sach on c.MaSach equals d.ID_Sach
                       join e in theLoai on c.MaTheLoai equals e.ID_TheLoai
                       join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                       select new HomeVM()
                       {
                           sanPhams = a,
                           SanPhamCT = b,
                           sachCT = c,
                           sach = d,
                           theLoai = e,
                           danhMucSach = f,
                       }).ToList();

            return Ok(homeVMs);

        }


    }
}
