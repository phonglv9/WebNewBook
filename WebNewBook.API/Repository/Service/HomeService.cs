using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class HomeService: IHomeService
    {
        private readonly dbcontext _dbContext;
        public HomeService(dbcontext dbContext)
        {
              _dbContext = dbContext;
        }
        public async Task<IEnumerable<HomeViewModel>> GetHomVM()
        {

            var sanPham =  _dbContext.SanPhams;
            var sanPhamCT =  _dbContext.SanPhamCTs;
            var sachCT =  _dbContext.SachCTs;
            var sach =  _dbContext.Sachs;
            var theLoai =  _dbContext.TheLoais;
            var danhMuc =  _dbContext.DanhMucSachs;
            var tacGia =  _dbContext.TacGias;
           
                var  homeVMs2 = (from a in sanPham
                       join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham
                       join c in sach on b.MaSach equals c.ID_Sach
                       join d in sachCT on c.ID_Sach equals d.MaSach
                       join e in theLoai on d.MaTheLoai equals e.ID_TheLoai
                       join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                       join g in tacGia on d.MaTacGia equals g.ID_TacGia
                       select new HomeViewModel
                       {
                           ID_SanPham = a.ID_SanPham,
                           TenSanPham = a.TenSanPham,
                           SoLuong = a.SoLuong,
                           GiaBan = a.GiaBan,
                           GiaGoc = a.GiaGoc,
                           HinhAnh = a.HinhAnh,
                           TenDanhMuc = f.TenDanhMuc,
                           idDanhMuc = f.ID_DanhMuc,
                           TrangThai = a.TrangThai,
                           NgayTao = a.NgayTao,
                    
                       }).ToList();

            List<HomeViewModel> lst = new List<HomeViewModel>();
            foreach (var item in homeVMs2.DistinctBy(c=>c.ID_SanPham).Where(c => c.TrangThai == 1).ToList())
            {
                lst.Add(item);
            }


            return lst;
        }
        public async Task<IEnumerable<ProductVM>> GetProductHome()
        {
            var sanPham = _dbContext.SanPhams;
            var sanPhamCT = _dbContext.SanPhamCTs;
            var sachCT = _dbContext.SachCTs;
            var sach = _dbContext.Sachs;
            var theLoai = _dbContext.TheLoais;
            var danhMuc = _dbContext.DanhMucSachs;
            var tacGia = _dbContext.TacGias;
           
           var  products = (from a in sanPham
                       join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham
                       join c in sach on b.MaSach equals c.ID_Sach
                       join d in sachCT on c.ID_Sach equals d.MaSach
                       join e in theLoai on d.MaTheLoai equals e.ID_TheLoai
                       join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                       join g in tacGia on d.MaTacGia equals g.ID_TacGia
                       select new ProductVM()
                       {
                           ID_SanPham = a.ID_SanPham,
                           TenSanPham = a.TenSanPham ,
                           SoLuong = a.SoLuong,
                           GiaBan = a.GiaBan,
                           GiaGoc = a.GiaGoc,
                           TrangThai = a.TrangThai,
                           HinhAnh = a.HinhAnh,
                           idTheLoai = e.ID_TheLoai,
                           TenTheLoai = e.TenTL,
                           idDanhMuc = f.ID_DanhMuc,
                            TenDanhMuc = f.TenDanhMuc,
                            idTacGia = g.ID_TacGia,
                            TenTacGia = g.HoVaTen,
                           
                           
             }).Where(c=>c.TrangThai == 1).ToList();

            List<ProductVM> lst = new List<ProductVM>();
            foreach (var item in products.DistinctBy(c => c.ID_SanPham).Where(c => c.TrangThai == 1).ToList())
            {
                lst.Add(item);
            }
            return lst;
        }
        public async Task<SanPhamChiTiet> GetProductDetail (string id)
        {

            
            var sanPhamCT = await _dbContext.SanPhamCTs.ToListAsync();
            var sachCT = await _dbContext.SachCTs.ToListAsync();
            var sach = await _dbContext.Sachs.ToListAsync();
            var sanPham = await _dbContext.SanPhams.ToListAsync();
            var theLoai = await _dbContext.TheLoais.ToListAsync();
            var tacGia = await _dbContext.TacGias.ToListAsync();

            List<SanPhamChiTiet> homeVMs = new List<SanPhamChiTiet>();
            homeVMs = (from a in sanPhamCT join b in sach on a.MaSach equals b.ID_Sach
                       join c in sachCT on b.ID_Sach equals c.MaSach
                       join d in sanPham on a.MaSanPham equals d.ID_SanPham
                       join j in tacGia on c.MaTacGia equals j.ID_TacGia
                       join k in theLoai on c.MaTheLoai equals k.ID_TheLoai
                       select new SanPhamChiTiet()
                       {
                          SanPhamCT=a,
                          sach=b,
                          sachCT=c,
                          sanPhams=d,
                          tacGia=j,
                          theLoai=k

                          
                       }).ToList();

            var x = homeVMs.Where(o => o.sanPhams.ID_SanPham == id).First();


            return x;
        }
        public async Task<List<TheLoai>> GetTheLoais()
        {
            

            return await _dbContext.TheLoais.ToListAsync();
        }
        public async Task<List<DanhMucSach>> GetDanhMucNavBar()
        {


            return await _dbContext.DanhMucSachs.Include(c=>c.TheLoais).ToListAsync();
        }
        public async Task<List<TacGia>> GetTacGias()
        {


            return await _dbContext.TacGias.ToListAsync();
        }
    }
}
