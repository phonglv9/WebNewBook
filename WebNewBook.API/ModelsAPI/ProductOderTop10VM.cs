namespace WebNewBook.API.ModelsAPI
{
    public class ProductOderTop10VM
    {
        public string ID_SanPham { get; set; }
        public string TenSanPham { get; set; }
        public double GiaBan { get; set; }
        public double GiaGoc { get; set; }
        public int SoLuong { get; set; }
        public string HinhAnh { get; set; }
        public int SoLuongDaBan { get; set; }
    }
}
