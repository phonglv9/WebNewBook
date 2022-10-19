using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebNewBook.Model;

namespace WebNewBook.API.Book
{
    public class BookModel
    {
        public string ID_Sach { get; set; }       
        public string MaNXB { get; set; }
        public string TenSach { get; set; }
        public string HinhAnh { get; set; }
        public int SoTrang { get; set; }
        public int TaiBan { get; set; }
        public double GiaBan { get; set; }
        public string? MoTa { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongKho { get; set; }
    }
    public class Publishers
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

}
