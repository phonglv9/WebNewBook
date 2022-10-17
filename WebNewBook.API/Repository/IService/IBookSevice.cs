using WebNewBook.API.Book;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IBookSevice
    {
        Task<List<BookModel>> GetListBook();
    }
}
