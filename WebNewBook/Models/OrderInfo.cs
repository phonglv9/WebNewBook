using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNewBook.Models
{
    public class OrderInfo
    {
        public long OrderId { get; set; }
        public double Amount { get; set; }
        public string MaKhachHang { get; set; }
        public string TenNguoiNhan { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string GhiChu { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? MaGiamGia { get; set; }
        public string OrderDesc { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public long PaymentTranId { get; set; }
        public string BankCode { get; set; }
        public string PayStatus { get; set; }
    }
}
