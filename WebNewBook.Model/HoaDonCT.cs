using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("HoaDonCT")]
    public class HoaDonCT
    {
        [Key]
        public string ID_HDCT { get; set; }
        [ForeignKey("SanPham")]
        public string MaSanPham { get; set; }
        [ForeignKey("HoaDon")]
        public string MaHoaDon { get; set; }
        public int SoLuong { get; set; }
        public virtual HoaDon? HoaDon { get; set; }
        public virtual SanPham? SanPham { get; set; }
        public virtual ICollection<PhieuTra>? PhieuTras { get; set; }
    }
}
