﻿using Microsoft.AspNetCore.Mvc;
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


            var sanPham = _dbContext.SanPhams;
            var sanPhamCT = _dbContext.SanPhamCTs;
            var sachCT = _dbContext.SachCTs;
            var sach = _dbContext.Sachs;
            var theLoai = _dbContext.TheLoais;
            var danhMuc = _dbContext.DanhMucSachs;
            var tacGia = _dbContext.TacGias;
            var nhaxuatban = _dbContext.NhaXuatBans;

            var products = (from a in sanPham
                            join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham
                            join c in sach on b.MaSach equals c.ID_Sach
                            join d in sachCT on c.ID_Sach equals d.MaSach
                            join e in theLoai on d.MaTheLoai equals e.ID_TheLoai
                            join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                            join g in tacGia on d.MaTacGia equals g.ID_TacGia
                            join x in nhaxuatban on c.MaNXB equals x.ID_NXB
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
                                sotrang=c.SoTrang,
                                taiban=c.TaiBan,
                                TenNhaXuatBan=x.TenXuatBan,
                                Mota= c.MoTa,


                            }).Where(c => c.TrangThai == 1).ToList();


            var lst = products.Where(c => c.ID_SanPham == id).FirstOrDefault();
              
            
            return lst;
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
