using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("Sach")]
    public class Sach
    {
        [Key]
        [Required]
        public string ID_Sach { get; set; }
        [ForeignKey("NhaXuatBan")]
        public string MaNXB { get; set; }
        public string TenSach { get; set; }
        public string HinhAnh { get; set; }
        [Range(1, 1000000)]
        public int SoTrang { get; set; }
        [Range(0, 100)]
        public int TaiBan { get; set; }
        [Range(1, 999999)]
        public double GiaBan { get; set; }
        public string? MoTa { get; set; }
        public int SoLuong { get; set; }
   
        public int TrangThai { get; set; }
        public virtual NhaXuatBan? NhaXuatBan { get; set; }
        public virtual ICollection<SachCT>? SachCTs { get; set; }
        public virtual ICollection<PhieuNhap>? PhieuNhaps { get; set; }
    }
}
