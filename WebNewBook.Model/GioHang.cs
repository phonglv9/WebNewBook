using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    public class GioHang
    {
        [Key]
        public string ID_GioHang { get; set; }
        [Required]
        public string HinhAnh { get; set; }
        [Required]
        public string TenSP { get; set; }
        [Required]
        public string emailKH { get; set; }
        [Required]
        public double DonGia { get; set; }
        public double? TongDonGia { get; set; }
        [Required]
        public int SoLuong { get; set; }
        public int? TongSoLuong { get; set; }
        public bool TrangThai { get; set; }
    }
}
