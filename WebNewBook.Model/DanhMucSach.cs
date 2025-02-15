﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebNewBook.Model
{
    [Table("DanhMuc")]
    public class DanhMucSach
    {
        [Key]
        public string ID_DanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public int TrangThai { get; set; }
        public virtual ICollection<TheLoai>? TheLoais { get; set; }
    }
}
