﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("PhieuNhap")]
    public class PhieuNhap
    {
        [Key]
        public string ID_PhieuNhap { get; set; }
        [ForeignKey("NhaCungCap")]
        public string MaNCC { get; set; }
        [ForeignKey("NhanVien")]
        public string MaNhanVien { get; set; }
        [ForeignKey("Sach")]
        public string MaSach { get; set; }
        public double GiaNhap { get; set; }
        public int SoLuongNhap { get; set; }
        public int SoLuongKho { get; set; }
        public DateTime NgayNhap { get; set; }
        public virtual NhaCungCap? NhaCungCap { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
        public virtual Sach? Sach { get; set; }
    }
}
