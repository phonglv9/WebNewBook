using WebNewBook.Model;

namespace WebNewBook.Services
{
	public interface IHeaderService
	{
        Task<List<DanhMucSach>> GetDMAsync();
        
    }
}
