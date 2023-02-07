using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using System.Data;
using WebNewBook.API.Common;
using WebNewBook.Model.APIModels;

namespace WebNewBook.API.Repository.Service
{
    public class HomeService : IHomeService
    {
        private readonly dbcontext _dbContext;
        public HomeService(dbcontext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<HomeViewModel>> GetHomVM()
        {
            var spcts = from sp in _dbContext.SanPhams.ToList()
                        join spct in _dbContext.SanPhamCTs.ToList() on sp.ID_SanPham equals spct.MaSanPham into spgroup
                        select new
                        {
                            SanPham = sp,
                            SanPhamCTs = spgroup
                        };


           
            var sanPham = new List<SanPhamViewModel>();
            spcts.ToList().ForEach(c =>
            {
                var singleType = c.SanPhamCTs.Count() == 1;
                var sp = new SanPhamViewModel();
                sp.SanPham = c.SanPham;
                sp.TheLoaiSP = !singleType ? 2 : c.SanPhamCTs.FirstOrDefault().SoLuongSach == 1 ? 1 : 3;
                sp.SoLuongSach = c.SanPhamCTs.FirstOrDefault().SoLuongSach;
                sanPham.Add(sp);
            });

            var sanPhamCT = _dbContext.SanPhamCTs;
            var sachCT = _dbContext.SachCTs;
            var sach = _dbContext.Sachs;
            var theLoai = _dbContext.TheLoais;
            var danhMuc = _dbContext.DanhMucSachs;
            var tacGia = _dbContext.TacGias;
            var sachTL = _dbContext.Sach_TheLoais;
            

            var homeVMs = (from a in sanPham
                            join b in sanPhamCT on a.SanPham.ID_SanPham equals b.MaSanPham                          
                            join c in sachCT on b.MaSachCT equals c.ID_SachCT
                           join d in sach on c.MaSach equals d.ID_Sach
                           join e in sachTL on d.ID_Sach equals e.MaSach
                           join g in theLoai on e.MaTheLoai equals g.ID_TheLoai
                           join f in danhMuc on g.MaDanhMuc equals f.ID_DanhMuc

                            select new HomeViewModel
                            {
                                ID_SanPham = a.SanPham.ID_SanPham,
                                TenSanPham = a.SanPham.TenSanPham,
                                SoLuong = a.SanPham.SoLuong,
                                GiaBan = a.SanPham.GiaBan,
                                GiaGoc = a.SanPham.GiaGoc,
                                HinhAnh = a.SanPham.HinhAnh,
                                TenDanhMuc = f.TenDanhMuc,
                                idDanhMuc = f.ID_DanhMuc,
                                TrangThai = a.SanPham.TrangThai,
                                NgayTao = a.SanPham.NgayTao,
                                TheLoai = a.TheLoaiSP,
                                SoLuongSach = a.SoLuongSach
                            }).ToList();

            List<HomeViewModel> lst = new List<HomeViewModel>();
            foreach (var item in homeVMs.DistinctBy(c => c.ID_SanPham).Where(c => c.TrangThai == 1).ToList())
            {
                lst.Add(item);
            }


            return lst;
        }
        public async Task<List<ProductOderTop10VM>> GetTopProduct10Oder()
        {

            List<ProductOderTop10VM> list = new List<ProductOderTop10VM>();
            DataSet dataSet = TextUtils.GetDataSet("spGetTop10ProductOder");
            DataTable dt = new DataTable();
            dt = dataSet.Tables[0];
            list = TextUtils.ConvertDataTable<ProductOderTop10VM>(dt);

            return list;

        }
        public async Task<IEnumerable<ProductVM>> GetProductHome()
        {
			var spcts = from sp in _dbContext.SanPhams.ToList()
						join spct in _dbContext.SanPhamCTs.ToList() on sp.ID_SanPham equals spct.MaSanPham into spgroup
						select new
						{
							SanPham = sp,
							SanPhamCTs = spgroup
						};



			var sanPham = new List<SanPhamViewModel>();
			spcts.ToList().ForEach(c =>
			{
				var singleType = c.SanPhamCTs.Count() == 1;
				var sp = new SanPhamViewModel();
				sp.SanPham = c.SanPham;
				sp.TheLoaiSP = !singleType ? 2 : c.SanPhamCTs.FirstOrDefault().SoLuongSach == 1 ? 1 : 3;
				sp.SoLuongSach = c.SanPhamCTs.FirstOrDefault().SoLuongSach;
				sanPham.Add(sp);
			});

			//var sanPham = _dbContext.SanPhams;
            var sanPhamCT = _dbContext.SanPhamCTs;
            var sachCT = _dbContext.SachCTs;
            var sach = _dbContext.Sachs;
            var theLoai = _dbContext.TheLoais;
            var danhMuc = _dbContext.DanhMucSachs;
            var tacGia = _dbContext.TacGias;
            var sachtacGia = _dbContext.Sach_TacGias;
            var sachTL = _dbContext.Sach_TheLoais;

            var products = (from a in sanPham
                            join b in sanPhamCT on a.SanPham.ID_SanPham equals b.MaSanPham
                            join c in sachCT on b.MaSachCT equals c.ID_SachCT
                            join d in sach on c.MaSach equals d.ID_Sach
                            join e in sachTL on d.ID_Sach equals e.MaSach
                            join g in theLoai on e.MaTheLoai equals g.ID_TheLoai
                            join f in danhMuc on g.MaDanhMuc equals f.ID_DanhMuc
                            join t in sachtacGia on d.ID_Sach equals t.MaSach
                            join tt in tacGia on t.MaTacGia equals tt.ID_TacGia


                            select new ProductVM()
                            {
                                ID_SanPham = a.SanPham.ID_SanPham,
                                TenSanPham = a.SanPham.TenSanPham,
                                SoLuong = a.SanPham.SoLuong,
                                GiaBan = a.SanPham.GiaBan,
                                GiaGoc = a.SanPham.GiaGoc,
                                TrangThai = a.SanPham.TrangThai,
                                HinhAnh = a.SanPham.HinhAnh,
                                idTheLoai = g.ID_TheLoai,
                                TenTheLoai = g.TenTL,
                                idDanhMuc = f.ID_DanhMuc,
                                TenDanhMuc = f.TenDanhMuc,
                                idTacGia = tt.ID_TacGia,
                                TenTacGia = tt.HoVaTen,
								TheLoai = a.TheLoaiSP,
								SoLuongSach = a.SoLuongSach

							}).Where(c => c.TrangThai == 1).ToList();

            List<ProductVM> lst = new List<ProductVM>();
            foreach (var item in products.DistinctBy(c => c.ID_SanPham).Where(c => c.TrangThai == 1).ToList())
            {
                lst.Add(item);
            }
            return lst;
        }

        public async Task<SanPhamChiTiet> GetProductDetail(string id)
        {


            var sanPham = _dbContext.SanPhams;
            var sanPhamCT = _dbContext.SanPhamCTs;
            var sachCT = _dbContext.SachCTs;
            var sach = _dbContext.Sachs;
            var theLoai = _dbContext.TheLoais;
            var danhMuc = _dbContext.DanhMucSachs;
            var tacGia = _dbContext.TacGias;
            var nhaxuatban = _dbContext.NhaXuatBans;
            var Sach_Theloai = _dbContext.Sach_TheLoais;
            var Sach_Tacgia = _dbContext.Sach_TacGias;

            var products = (from a in sanPham
                            join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham

                            join d in sachCT on b.MaSachCT equals d.ID_SachCT
                            join x in nhaxuatban on d.MaNXB equals x.ID_NXB
                            join c in sach on d.MaSach equals c.ID_Sach
                            join p in Sach_Theloai on c.ID_Sach equals p.MaSach
                            join l in Sach_Tacgia on c.ID_Sach equals l.MaSach
                            join e in theLoai on p.MaTheLoai equals e.ID_TheLoai
                            join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                            join g in tacGia on l.MaTacGia equals g.ID_TacGia

                            select new SanPhamChiTiet()
                            {
                                ID_SanPham = a.ID_SanPham,
                                TenSanPham = a.TenSanPham,
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
                                sotrang = c.SoTrang,
                                taiban = d.TaiBan,
                                TenNhaXuatBan = x.TenXuatBan,
                                TenSach = c.TenSach,
                                Mota = c.MoTa,


                            }).Where(c => c.TrangThai == 1).ToList();





            return products.Where(c => c.ID_SanPham == id).FirstOrDefault();
        }
        
        public async Task<List<TheLoai>> GetTheLoais()
        {


            return await _dbContext.TheLoais.ToListAsync();
        }
        public async Task<List<DanhMucSach>> GetDanhMucNavBar()
        {


            return await _dbContext.DanhMucSachs.Include(c => c.TheLoais).ToListAsync();
        }
        public async Task<List<TacGia>> GetTacGias()
        {


            return await _dbContext.TacGias.ToListAsync();
        }

        public async Task<List<SanPhamChiTiet>> GetTheLoaisCT(string id)
        {
            var sanPham = _dbContext.SanPhams;
            var sanPhamCT = _dbContext.SanPhamCTs;
            var sachCT = _dbContext.SachCTs;
            var sach = _dbContext.Sachs;
            var theLoai = _dbContext.TheLoais;
            var danhMuc = _dbContext.DanhMucSachs;
            var tacGia = _dbContext.TacGias;
            var nhaxuatban = _dbContext.NhaXuatBans;
            var Sach_Theloai = _dbContext.Sach_TheLoais;
            var Sach_Tacgia = _dbContext.Sach_TacGias;

            var products = (from a in sanPham
                            join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham

                            join d in sachCT on b.MaSachCT equals d.ID_SachCT
                            join x in nhaxuatban on d.MaNXB equals x.ID_NXB
                            join c in sach on d.MaSach equals c.ID_Sach
                            join p in Sach_Theloai on c.ID_Sach equals p.MaSach
                            join l in Sach_Tacgia on c.ID_Sach equals l.MaSach
                            join e in theLoai on p.MaTheLoai equals e.ID_TheLoai
                            join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                            join g in tacGia on l.MaTacGia equals g.ID_TacGia
                            select new SanPhamChiTiet()
                            {
                                ID_SanPham = a.ID_SanPham,
                                TenSanPham = a.TenSanPham,
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
                                sotrang = c.SoTrang,
                                taiban = d.TaiBan,
                                TenNhaXuatBan = x.TenXuatBan,
                                Mota = c.MoTa,
                                TenSach = c.TenSach



                            }).Where(c => c.TrangThai == 1).ToList();
           


            var lst = products.Where(c => c.ID_SanPham == id).ToList();
            return lst;
        }
    }
}
