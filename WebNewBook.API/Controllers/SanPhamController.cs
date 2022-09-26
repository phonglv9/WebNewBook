using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SanPhamController : Controller
    {
        private readonly ISanPhamService sanPhamService;
        public SanPhamController(ISanPhamService sanPhamService)
        {
            this.sanPhamService = sanPhamService;
        }

        [HttpGet]
        public async Task<IEnumerable<SanPham>> GetSanPhamAsync()
        {
            var sp = await sanPhamService.GetSanPhamAsync();
            return sp;
        }

        [HttpGet("{id}")]
        public async Task<SanPham?> GetSanPhamAsync(string id)
        {
            var sp = await sanPhamService.GetSanPhamAsync(id);
            return sp;
        }

        [HttpPost]
        public async Task<ActionResult> AddSanPhamAsync(SanPham sp, [FromRoute]IEnumerable<string> Sachs)
        {
            try
            {
                await sanPhamService.AddSanPhamAsync(sp, Sachs);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSanPhamAsync(SanPham sp)
        {
            try
            {
                await sanPhamService.UpdateSanPhamAsync(sp);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
