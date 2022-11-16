using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("KhachHang")]
    public class KhachHang
    {
        [Key]
        public string ID_KhachHang { get; set; }
        [Required]
        public string HoVaTen { get; set; }
        [EmailAddress]
        public string ?Email { get; set; }
        public string ?SDT { get; set; }
        public string ?DiaChi { get; set; }
        public DateTime ?NgaySinh { get; set; }
        [MinLength(5)]
        [MaxLength(20)]
        public string ?MatKhau { get; set; }
        public int DiemTichLuy { get; set; }
        [Range(0, 1)]
        public int TrangThai { get; set; }
        public virtual ICollection<HoaDon>? HoaDons { get; set; }
        public virtual ICollection<VoucherCT>? VoucherCTs { get; set; }
        public virtual ICollection<Fpoint>? Fpoints { get; set; }
    }
}
