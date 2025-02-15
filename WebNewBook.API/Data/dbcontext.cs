﻿using Microsoft.EntityFrameworkCore;
using WebNewBook.Model;

namespace WebNewBook.API.Data
{
    public class dbcontext: DbContext
    {
        public dbcontext(DbContextOptions<dbcontext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //modelbuilder.Entity<TheLoai>().HasIndex(c => c.TenTL).IsUnique();
            //modelbuilder.Entity<KhachHang>().HasIndex(c => c.Email).IsUnique();
            //modelbuilder.Entity<NhanVien>().HasIndex(c => c.Email).IsUnique();
            modelbuilder.Entity<Sach>().HasIndex(c => c.TenSach).IsUnique();
            modelbuilder.Entity<SanPham>().HasIndex(c => c.TenSanPham).IsUnique();
            modelbuilder.Entity<SachCT>().HasIndex(c => new { c.MaNXB, c.TaiBan, c.MaSach, c.BiaMem }).IsUnique();
            modelbuilder.Entity<SanPhamCT>().HasIndex(c => new { c.MaSachCT, c.SoLuongSach }).IsUnique().HasFilter("[SoLuongSach] > 1");
            //modelbuilder.Entity<SanPhamCT>().HasIndex(c => c.MaSanPham).IsUnique().HasFilter("[SoLuongSach] > 1");
        }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<HoaDonCT> HoaDonCTs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public DbSet<Sach> Sachs { get; set; }
        public DbSet<SachCT> SachCTs { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<VoucherCT> VoucherCTs { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        //public DbSet<PhieuGiamGiaSP> PhieuGiamGiaSPs { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<PhieuTra> PhieuTras { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<SanPhamCT> SanPhamCTs { get; set; }
        public DbSet<DanhMucSach> DanhMucSachs { get; set; }
        public DbSet<Fpoint> Fpoints { get; set; }
        public DbSet<Sach_TacGia> Sach_TacGias { get; set; }
        public DbSet<Sach_TheLoai> Sach_TheLoais { get; set; }
    }
}
