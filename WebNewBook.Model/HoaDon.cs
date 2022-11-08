using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebNewBook.Model
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public string ID_HoaDon { get; set; }
        [ForeignKey("KhachHang")]
        public string MaKhachHang { get; set; }
        public string TenNguoiNhan { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string? GhiChu { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? MaGiamGia { get; set; }
        public DateTime NgayMua { get; set; }
        public double TongTien { get; set; }
        [Range(0, 5)]
        public int TrangThai { get; set; }
        
        public virtual KhachHang? KhachHang { get; set; }
        public virtual ICollection<HoaDonCT>? HoaDonCTs { get; set; }
        public virtual ICollection<PhieuTra>? PhieuTras { get; set; }
        public virtual Fpoint? Fpoint { get; set; }

    }
}
