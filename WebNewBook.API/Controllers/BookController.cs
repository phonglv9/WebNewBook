using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;
using static WebNewBook.API.Repository.Service.BookService;

namespace WebNewBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,NhanVien")]
    public class BookController : ControllerBase
    {
        private readonly IBookSevice _BookService;
        public BookController(IBookSevice BookService)
        {
            _BookService = BookService;
        }

        [HttpGet]
        public async Task<List<Sach>> GetlistSach()
        {
            var Model = await _BookService.GetListSach();
            return Model.ToList();
        }


        [HttpGet("sachct")]
        public async Task<List<SachCT>> GetSachCT()
        {
            var Model = await _BookService.GetSachCT();
            return Model;
        }

        [HttpGet("sachct/{Id}")]
        public async Task<SachCT> GetSachCT(string id)
        {
            var Model = await _BookService.GetSachCT(id);
            return Model;
        }

        [HttpGet("sach_sachct")]
        public IEnumerable<Sach_SachCT> GetSach_SachCT()
        {
            var Model = _BookService.GetSach_SachCT();
            return Model;
        }

        [HttpGet("sachTG/{Id}")]
        public async Task<List<Sach_TacGia>> GetSachTG(string Id)
        {
            var Model = await _BookService.GetSachTG_TL<Sach_TacGia>(Id);
            return Model;
        }

        [HttpGet("sachTL/{Id}")]
        public async Task<List<Sach_TheLoai>> GetSachTL(string Id)
        {
            var Model = await _BookService.GetSachTG_TL<Sach_TheLoai>(Id);
            return Model;
        }

        [HttpPost]
        public async Task<ActionResult> CreateEvent(SachAPI input)
        {
            try
            {
                await _BookService.CreateSach(input);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("sachct")]
        public async Task<ActionResult> CreateBook(SachCT input)
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
        public async Task<ActionResult> UpdateSach(SachAPI input)
        {
            try
            {
                await _BookService.UpdateSach(input);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("sachct")]
        public async Task<ActionResult> UpdateBook(SachCT input)
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

        [HttpPut("update_status/{Id}")]
        public async Task<ActionResult> DeleteNews(string Id)
        {
            try
            {
                await _BookService.DeteleBook(Id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
