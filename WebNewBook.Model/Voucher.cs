using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    public class Voucher
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public DateTime Createdate { get; set; }
        public string TenPhatHanh { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double MenhGia { get; set; }
        public double MenhGiaDieuKien { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        [ForeignKey("NhanVien")]
        public string MaNhanVien { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
        public virtual ICollection<VoucherCT>? VoucherCTs { get; set; }



    }
}
