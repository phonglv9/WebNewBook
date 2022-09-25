using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("SachCT")]
    public class SachCT
    {
        [Key]
        public string ID_SachCT { get; set; }
        [ForeignKey("Sach")]
        public string MaSach { get; set; }
        [ForeignKey("TheLoai")]
        public string MaTheLoai { get; set; }
        [ForeignKey("TacGia")]
        public string MaTacGia { get; set; }
        public virtual Sach? Sach { get; set; }
        public virtual TheLoai? TheLoai { get; set; }
        public virtual TacGia? TacGia { get; set; }
    }
}
