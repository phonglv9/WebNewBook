using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("NhanVien")]
    public class NhanVien
    {
        [Key]
        public string ID_NhanVien { get; set; }
        public string HoVaTen { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string HinhAnh { get; set; }
        public DateTime NgaySinh { get; set; }
        [MinLength(5)]
        [MaxLength(20)]
        public string MatKhau { get; set; }
        public bool Quyen { get; set; }
        public int TrangThai { get; set; }
        public virtual ICollection<PhieuNhap>? PhieuNhaps { get; set; }
        public virtual ICollection<Voucher>? Vouchers { get; set; }
    }
}
