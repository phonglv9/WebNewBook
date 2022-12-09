using WebNewBook.Model;

namespace WebNewBook.API.ModelsAPI
{
	public class SanPhamChiTiet
	{
        public string ID_SanPham { get; set; }
        public string TenSanPham { get; set; }
        public int SoLuong { get; set; }
        public double GiaBan { get; set; }
        public double GiaGoc { get; set; }
        public string HinhAnh { get; set; }
        public string idDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public string idTacGia { get; set; }
        public string TenTacGia { get; set; }
        public string Mota { get; set; }
        public string idTheLoai { get; set; }
        public string TenTheLoai { get; set; }
        public string TenNhaXuatBan { get; set; }
        public int sotrang { get; set; }
        public int taiban { get; set; }
        public int TrangThai { get; set; }


    }
}
