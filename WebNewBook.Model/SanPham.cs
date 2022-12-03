using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        public string ID_SanPham { get; set; }
        [ForeignKey("PhieuGiamGiaSP")]
        public string? MaPhieuGiamGiaSP { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; } 
        public double GiaBan { get; set; }
        public double GiaGoc { get; set; }
        public string HinhAnh { get; set; }
        public int TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public virtual PhieuGiamGiaSP? PhieuGiamGiaSP { get; set; }
        public virtual ICollection<HoaDonCT>? HoaDonCTs { get; set; }
        public virtual ICollection<SanPhamCT>? SanPhamCTs { get; set; }
    }
}
