using WebNewBook.Model;

namespace WebNewBook.API.ModelsAPI
{
	public class SanPhamChiTiet
	{
        public SanPhamCT SanPhamCT { get; set; }
        public SachCT sachCT { get; set; } 
        public Sach sach { get; set; }
        public SanPham sanPhams { get; set; }
        public TheLoai theLoai { get; set; } 
        public TacGia tacGia { get; set; } 

    }
}
