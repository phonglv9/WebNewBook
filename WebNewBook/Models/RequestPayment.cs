using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNewBook.Models
{
    public class RequestPayment
    {
        public string SoTien { get; set; }
        public string NoiDungThanhToan { get; set; }
        public string NgonNgu { get; set; }
        public string NganHang { get; set; }
        public string LoaiHangHoa { get; set; }
        public string ThoiHanThanhToan { get; set; }

        public string HDonDienThoai { get; set; }
        public string HDonEmail { get; set; }
        public string HDonHoTen { get; set; }
        public string HDonTinhThanhPho { get; set; }
        public string HDonQuocGia { get; set; }

        public string HDDTTenKhachHang { get; set; }
        public string HDDTDienThoai { get; set; }
        public string HDDTEmail { get; set; }
        public string HDDTCongTy { get; set; }
        public string HDDTMaSoThue { get; set; }
        public string HDDTDiaChi { get; set; }
        public string HDDTLoaiHoaDon { get; set; }
    }
}
