﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("PhieuGiamGia")]
    public class VoucherCT
    {
        [Key]
        [Required]
        public string Id { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public int HinhThuc { get; set; }
        public int SoLuong { get; set; }
        public int Diemdoi { get; set; }
        public DateTime Ngay { get; set; }
        public DateTime NgayHetHan { get; set; }
        public int TrangThai { get; set; }
        [ForeignKey("KhachHang")]
        public string MaKhachHang { get; set; }
        public virtual KhachHang? KhachHang { get; set; }
        [ForeignKey("Voucher")]
        public string MaVoucher { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}