using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;
using static WebNewBook.API.Repository.Service.BookService;

namespace WebNewBook.API.Repository.IService
{
    public interface IBookSevice
    {
        Task<IEnumerable<Sach>> GetListSach();
        Task<List<SachCT>> GetSachCT();
        Task CreateSach(SachAPI input);
        Task CreateBook(SachCT input);
        Task UpdateSach(SachAPI input);
        Task DeleteSach(string input);
        Task UpdateBook(SachCT input);
        Task DeteleBook(string ID);
        Task<dynamic> GetSachTG_TL<T>(string id);
        Task<SachCT> GetSachCT(string id);
        IEnumerable<Sach_SachCT> GetSach_SachCT();
        List<SachCTViewModel> GetSachCTViewModels();
        List<SachViewModel> GetSachViewModels();
    }
}
