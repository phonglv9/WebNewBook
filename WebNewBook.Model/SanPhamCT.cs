using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("SanPhamCT")]
    public class SanPhamCT
    {
        [Key]
        public string ID_SanPhamCT { get; set; }
        [ForeignKey("SanPham")]
        public string MaSanPham { get; set; }
        [ForeignKey("PhieuNhap")]
        public string MaPhieuNhap { get; set; }
        public int SoLuong { get; set; }
        public int TrangThai { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual PhieuNhap PhieuNhap { get; set; }
    }
}
