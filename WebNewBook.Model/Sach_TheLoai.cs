using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("Sach_TheLoai")]
    public class Sach_TheLoai
    {
        [Key]
        public string ID_SachTheLoai { get; set; }
        [ForeignKey("Sach")]
        public string MaSach { get; set; }
        [ForeignKey("TheLoai")]
        public string MaTheLoai { get; set; }
        public virtual Sach? Sach { get; set; }
        public virtual TheLoai? TheLoai { get; set; }
    }
}
