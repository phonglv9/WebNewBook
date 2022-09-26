using WebNewBook.Model;

namespace WebNewBook.API.ModelsAPI
{
    public class HomeVM
    {
        public SanPham sanPhams { get; set; } = new SanPham();
        public SanPhamCT SanPhamCT { get; set; } =  new SanPhamCT();
        public SachCT sachCT { get; set; } =  new SachCT();
        public Sach sach { get; set; } =  new Sach();
        public TheLoai theLoai { get; set; } =  new TheLoai();
        public DanhMucSach danhMucSach { get; set; } =  new DanhMucSach();
        public TacGia tacGia { get; set; } =  new TacGia();


    }
}
