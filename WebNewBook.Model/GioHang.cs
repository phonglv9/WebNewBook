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
        public string Maasp { get; set; }
        [Required]
        public string Tensp { get; set; }
        [Required]
        public string emailKH { get; set; }
        [Required]
        public double DonGia { get; set; }
        public double? ThanhTien { get; set; }
        [Required]
        public int Soluong { get; set; }
       
       
    }
}
