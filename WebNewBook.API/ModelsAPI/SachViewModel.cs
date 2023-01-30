using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model.APIModels
{
    public class SachViewModel
    {
        public Sach? Sach { get; set; }
        public List<string> TacGia { get; set; } = new List<string>();
        public List<string> TheLoai { get; set; } = new List<string>();
    }
}
