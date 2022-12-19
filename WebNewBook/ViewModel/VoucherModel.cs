using WebNewBook.Model;
namespace WebNewBook.ViewModel
{
    public class VoucherModel
    {
        public Voucher Voucher { get; set; }= new Voucher();
        public VoucherCT voucherCT { get; set; } = new VoucherCT();
        public IFormFile File { get; set; }
        public IEnumerable<string>? KhachHangs { get; set; }
    }
}
