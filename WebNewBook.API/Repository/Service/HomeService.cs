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
        public async Task<List<HomeVM>> GetHomVM()
        {

            var sanPham = await _dbContext.SanPhams.ToListAsync();
            var sanPhamCT = await _dbContext.SanPhamCTs.ToListAsync();
            var sachCT = await _dbContext.SachCTs.ToListAsync();
            var sach = await _dbContext.Sachs.ToListAsync();
            var theLoai = await _dbContext.TheLoais.ToListAsync();
            var danhMuc = await _dbContext.DanhMucSachs.ToListAsync();
            List<HomeVM> homeVMs = new List<HomeVM>();
            homeVMs = (from a in sanPham
                       join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham
                       join c in sach on b.MaSach equals c.ID_Sach
                       join d in sachCT on c.ID_Sach equals d.MaSach
                       join e in theLoai on d.MaTheLoai equals e.ID_TheLoai
                       join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                       select new HomeVM()
                       {
                           sanPhams = a,
                           SanPhamCT = b,
                           sach = c,
                           sachCT = d,
                           theLoai = e,
                           danhMucSach = f,
                       }).ToList();


            

            return homeVMs;
        }
        public async Task<List<HomeVM>> GetProductHome()
        {
            var sanPham = await _dbContext.SanPhams.ToListAsync();
            var sanPhamCT = await _dbContext.SanPhamCTs.ToListAsync();
            var sachCT = await _dbContext.SachCTs.ToListAsync();
            var sach = await _dbContext.Sachs.ToListAsync();
            var theLoai = await _dbContext.TheLoais.ToListAsync();
            var danhMuc = await _dbContext.DanhMucSachs.ToListAsync();
            var tacGia = await _dbContext.TacGias.ToListAsync();
            List<HomeVM> homeVMs = new List<HomeVM>();
            homeVMs = (from a in sanPham
                       join b in sanPhamCT on a.ID_SanPham equals b.MaSanPham
                       join c in sach on b.MaSach equals c.ID_Sach
                       join d in sachCT on c.ID_Sach equals d.MaSach
                       join e in theLoai on d.MaTheLoai equals e.ID_TheLoai
                       join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                       join g in tacGia on d.MaTacGia equals g.ID_TacGia
                       select new HomeVM()
                       {
                           sanPhams = a,
                           SanPhamCT = b,
                           sach = c,
                           sachCT = d,
                           theLoai = e,
                           danhMucSach = f,
                           tacGia = g,
                       }).ToList();
                     

            return  homeVMs;
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
        public async Task<List<DanhMucSach>> GetDanhMucs()
        {


            return await _dbContext.DanhMucSachs.ToListAsync();
        }
        public async Task<List<TacGia>> GetTacGias()
        {


            return await _dbContext.TacGias.ToListAsync();
        }
    }
}
