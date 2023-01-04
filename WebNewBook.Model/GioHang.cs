using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("GioHang")]
    public class GioHang
    {
        [Key]
        public string ID_GioHang { get; set; }
        [ForeignKey("KhachHang")]
        public string MaKhachHang { get; set; }
        public string MaSanPham { get; set; }
        public int SoLuong { get; set; }
        public virtual KhachHang? KhachHang { get; set; }
    }
}
