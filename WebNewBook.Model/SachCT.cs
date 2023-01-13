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
        [ForeignKey("NhaXuatBan")]
        public string MaNXB { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuong { get; set; }
        [Range(1, 999999)]
        public double GiaBan { get; set; }
        [Range(0, 100)]
        public int TaiBan { get; set; }
        //public bool BiaMem { get; set; }
        public int TrangThai { get; set; }
        public virtual Sach? Sach { get; set; }
        public virtual NhaXuatBan? NhaXuatBan { get; set; }
    }
}
