using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("Sach")]
    public class Sach
    {
        [Key]
        [Required]
        public string ID_Sach { get; set; }
        public string TenSach { get; set; }
        [Range(1, 1000000)]
        public int SoTrang { get; set; }
        public string MoTa { get; set; }
    }
}
