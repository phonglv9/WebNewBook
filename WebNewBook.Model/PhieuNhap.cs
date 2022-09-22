using System;
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
        [ForeignKey("NhanVien")]
        public string MaNhanVien { get; set; }
        [ForeignKey("SachCT")]
        public string MaSachCT { get; set; }
        public double GiaNhap { get; set; }
        public int SoLuongNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
        public virtual SachCT? SachCT { get; set; }
    }
}
