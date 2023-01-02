using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("TacGia")]
    public class TacGia
    {
        [Key]
        public string ID_TacGia { get; set; }
        public string HoVaTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string QueQuan { get; set; }
        [Range(0, 1)]
        public int TrangThai { get; set; }
    }
}
