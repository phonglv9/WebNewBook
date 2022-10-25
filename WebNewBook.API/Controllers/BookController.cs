using Microsoft.AspNetCore.Authorization;
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
        /// <summary>
        /// lấy danh sách 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetlistBook")]
        public async Task<List<BookModel>> GetlistBook()
        {
            var Model = await _BookService.GetListBook();
            return Model;
        }
        /// <summary>
        /// Thêm mới sách
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("CreateBook")]
        public async Task<Sach> CreateEvent(CreateBookModel input)
        {   
            return await _BookService.CreateBook(input);
        }
        /// <summary>
        /// Sửa sách
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("UpdateBook")]
        public async Task<string> UpdateBook(UpdateBook input)
        {
            return await _BookService.UpdateBook(input);
        }
        /// <summary>
        /// Xoá sách
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost("DeleteNews/{ID}")]
        //[Authorize]
        public async Task<string> DeleteNews(string ID)
        {
            return await _BookService.DeteleBook(ID);
        }
    }
}
