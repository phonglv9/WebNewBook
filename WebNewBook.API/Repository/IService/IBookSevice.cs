using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IBookSevice
    {
        Task<IEnumerable<Sach>> GetListBook();
        Task CreateBook(SachAPI input);
        Task UpdateBook(SachAPI input);
        Task DeteleBook(string ID);

        
    }
}
