using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Book;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookSevice _BookService;
        public BookController(IBookSevice BookService)
        {
            _BookService = BookService;
        }
        [HttpGet("GetlistBook")]
        public async Task<List<BookModel>> GetlistBook()
        {
            var Model = await _BookService.GetListBook();
            return Model;
        }
    }
}
