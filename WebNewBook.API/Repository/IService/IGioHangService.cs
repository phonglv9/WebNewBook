using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
	public interface IGioHangService
	{
        Task<List<HomeVM>> VM();
        Task<SanPham> GetSanPham(string id);
    }
}
