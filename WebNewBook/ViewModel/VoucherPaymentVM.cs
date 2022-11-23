namespace WebNewBook.ViewModel
{
    public class VoucherPaymentVM
    {
        public string ID_Voucher { get; set; }
        public string TenPhatHanh { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime ?NgayHetHan { get; set; }
        public double MenhGia { get; set; }
        public double MenhGiaDieuKien { get; set; }

    }
}
