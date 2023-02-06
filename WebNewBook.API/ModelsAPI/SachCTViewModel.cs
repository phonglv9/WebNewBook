using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model.APIModels
{
    public class SachCTViewModel
    {
        public SachCT SachCT { get; set; }
        public string TenSach { get; set; }
        public string NXB { get; set; }
        public string MaSach { get; set; }
    }
}
