using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("Sach_TacGia")]
    public class Sach_TacGia
    {
        [Key]
        public string ID_SachTacGia { get; set; }
        [ForeignKey("Sach")]
        public string MaSach { get; set; }
        [ForeignKey("TacGia")]
        public string MaTacGia { get; set; }
        public virtual Sach? Sach { get; set; }
        public virtual TacGia? TacGia { get; set; }
    }
}
