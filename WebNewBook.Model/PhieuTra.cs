using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("PhieuTra")]
    public class PhieuTra
    {
        [Key]
        public string ID_PhieuTra { get; set; }
        [ForeignKey("HoaDon")]
        public string MaHoaDon { get; set; }
        [ForeignKey("HoaDonCT")]
        public string MaHoaDonCT { get; set; }
        public int SoLuong { get; set; }
        public int TheLoai { get; set; }
        public string? MoTa { get; set; }
        public int TrangThai { get; set; }
        public virtual HoaDon HoaDon { get; set; }
        public virtual HoaDonCT HoaDonCT { get; set; }
    }
}
