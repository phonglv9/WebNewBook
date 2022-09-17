using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public string ID_HoaDon { get; set; }
        [ForeignKey("KhachHang")]
        public string MaKhachHang { get; set; }
        public string MaGiamGia { get; set; }
        public DateTime NgayMua { get; set; }
        public double TongTien { get; set; }
        [Range(0, 2)]
        public int TrangThai { get; set; }
        public virtual KhachHang? KhachHang { get; set; }
        public virtual ICollection<HoaDonCT>? HoaDonCTs { get; set; }
        public virtual ICollection<PhieuTra>? PhieuTras { get; set; }

    }
}
