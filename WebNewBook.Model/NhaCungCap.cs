using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("NhaCungCap")]
    public class NhaCungCap
    {
        [Key]
        public string ID_NCC { get; set; }
        public string TenNCC { get; set; }
        public int TrangThai { get; set; }
        public virtual ICollection<PhieuNhap>? PhieuNhaps { get; set; }
    }
}
