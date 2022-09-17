using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("TheLoai")]
    public class TheLoai
    {
        [Key]
        public string ID_TheLoai { get; set; }
        [ForeignKey("DanhMucSach")]
        public string MaDanhMuc { get; set; }
        public string TenTL { get; set; }
        public virtual DanhMucSach? DanhMucSach { get; set; }
        public virtual ICollection<SachCT>? SachCTs { get; set; }
    }
}
