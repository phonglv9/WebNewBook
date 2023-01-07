using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
	public interface IGioHangService
	{
        
        Task<SanPham> GetSanPham(string id);
        Task<int> getSP(string id);
        Task<string>XoakhoiGioHang(string id,string namekh);
       Task XoaGioHangKH(string email);
        Task<int> AddGioHangAsync(string HinhAnh, int SoLuongs, string emailKH, string idsp);
        Task<List<ModelCart>> GetlistGH();
        Task<int> Updatenumber(string id,int soluongmoi, string namekh, string update);
        Task<int> ChecksoluongCart();
      

    }
}
