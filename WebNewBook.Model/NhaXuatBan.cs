using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("NhaXuatBan")]
    public class NhaXuatBan
    {
        [Key]
        public string ID_NXB { get; set; }
        public string TenXuatBan { get; set; }
        [Range(0, 1)]
        public int TrangThai { get; set; }
    }
}
