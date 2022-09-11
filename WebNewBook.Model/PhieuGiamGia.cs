using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("PhieuGiamGia")]
    public class PhieuGiamGia
    {
        [Key]
        public string ID_PhieuGiamGia { get; set; }
        [ForeignKey("KhachHang")]
        public string MaKhachHang { get; set; }
        public string TenPhieu { get; set; }
        public int GiaTri { get; set; }
        public int SoLuong { get; set; }
        public int TheLoai { get; set; }
        public DateTime NgayHetHan { get; set; }
        public virtual KhachHang KhachHang { get; set; }
    }
}
