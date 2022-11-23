using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookSevice _BookService;
        public BookController(IBookSevice BookService)
        {
            _BookService = BookService;
        }

        [HttpGet]
        public async Task<List<Sach>> GetlistBook()
        {
            var Model = await _BookService.GetListBook();
            return Model.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> CreateEvent(SachAPI input)
        {
            try
            {
                await _BookService.CreateBook(input);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBook(SachAPI input)
        {
            try
            {
                await _BookService.UpdateBook(input);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update_status")]
        //[Authorize]
        public async Task<ActionResult> DeleteNews(SachAPI sach)
        {
            try
            {
                await _BookService.UpdateBook(sach);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
