using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("PhieuGiamGiaSP")]
    public class PhieuGiamGiaSP
    {
        [Key]
        public string ID_PhieuGiamGiaSP { get; set; }
        public string TenPhieu { get; set; }
        public double GiaTri { get; set; }
        public double SPChoPhep { get; set; }
        public int TheLoai { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayHetHan { get; set; }
        public virtual ICollection<SanPham>? SanPhams { get; set; }
    }
}
