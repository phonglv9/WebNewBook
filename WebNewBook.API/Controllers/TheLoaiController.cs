using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheLoaiController : Controller
    {
        private readonly ITheLoaiService theLoaiService;
        public TheLoaiController(ITheLoaiService theLoaiService)
        {
            this.theLoaiService = theLoaiService;
        }

        [HttpGet]
        public async Task<IEnumerable<TheLoai>> GetTheLoaisAsync()
        {
            var tl = await theLoaiService.GetTheLoaiAsync();
            return tl;
        }

        [HttpGet("{id}")]
        public async Task<TheLoai?> GetTacGiaAsync(string id)
        {
            var tl = await theLoaiService.GetTheLoaiAsync(id);
            return tl;
        }

        [HttpPost]
        public async Task<ActionResult> AddTacGiaAsync(TheLoai tl)
        {
            try
            {
                await theLoaiService.AddTheLoaiAsync(tl);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTheLoaiAsync(TheLoai tl)
        {
            try
            {
                await theLoaiService.UpdateTheLoaiAsync(tl);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> DeleteTheLoaiAsync(string id)
        {
            try
            {
                await theLoaiService.DeleteTheLoaiAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
