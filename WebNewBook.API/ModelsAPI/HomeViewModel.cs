﻿using WebNewBook.Model;

namespace WebNewBook.API.ModelsAPI
{
    public class HomeViewModel
    {
        public string ID_SanPham { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public double GiaBan { get; set; }
        public double GiaGoc { get; set; }
        public string HinhAnh { get; set; }
        public int TrangThai { get; set; }
        public string idDanhMuc { get; set; }
        public string TenDanhMuc { get; set; } 
        public DateTime NgayTao { get; set; }
        public int TheLoai { get; set; }//1 - Sach | 2 - Bo sach | 3 - Bo sach cung loai
        public int SoLuongSach { get; set; }
    }
}
