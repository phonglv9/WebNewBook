using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    public class Fpoint
    {
        [ForeignKey("HoaDon")]
        public string Id { get; set; }
        public string NameHanhDong { get; set; }
        public DateTime CreatDate { get; set; }
        public int Hanhdong { get; set; }
        public double Diemtichluy { get; set; }
        public int TrangThai { get; set; }
        [ForeignKey("KhachHang")]
        public string? MaKhachHang { get; set; }
        public virtual KhachHang? KhachHang { get; set; }
        public virtual HoaDon? HoaDon { get; set; }

    }
}
