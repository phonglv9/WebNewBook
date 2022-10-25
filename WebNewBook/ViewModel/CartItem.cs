namespace WebNewBook.Models
{
	public class CartItem
	{
		public string Maasp { get; set; }
		public string Tensp { get; set; }
        public double DonGia { get; set; }
        public int Soluong { get; set; }
        public double ThanhTien
        {
            get
            {
                return Soluong * DonGia;
            }
        }


    }
}
