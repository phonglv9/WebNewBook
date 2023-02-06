using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model.APIModels
{
    public class SanPhamAPI
    {
        public SanPham? SanPham { get; set; }
        public IEnumerable<string>? Sachs { get; set; }
        [Range(0, 100)]
        public double GiamGia { get; set; }
        [Range(1, 100)]
        public int SLSachCT { get; set; } = 1;
    }
}
