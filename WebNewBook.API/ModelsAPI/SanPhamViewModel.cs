using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model.APIModels
{
    public class SanPhamViewModel
    {
        public SanPham? SanPham { get; set; }
        public int TheLoaiSP { get; set; } //1 - Sach | 2 - Bo sach | 3 - Bo sach cung loai
        public int SoLuongSach { get; set; }
    }
}
