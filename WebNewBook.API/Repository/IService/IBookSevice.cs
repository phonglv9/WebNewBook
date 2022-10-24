using WebNewBook.API.Book;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IBookSevice
    {
        Task<List<BookModel>> GetListBook();
        Task<Sach> CreateBook(CreateBookModel input);
        Task<string> UpdateBook(UpdateBook input);
        Task<string> DeteleBook(string ID);

        
    }
}
