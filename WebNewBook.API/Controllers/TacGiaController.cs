using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacGiaController : Controller
    {
        private readonly ITacGiaService tacGiaService;
        public TacGiaController(ITacGiaService tacGiaService)
        {
            this.tacGiaService = tacGiaService;
        }

        [HttpGet]
        public async Task<IEnumerable<TacGia>> GetTacGiasAsync()
        {
            var tg = await tacGiaService.GetTacGiaAsync();
            return tg;
        }

        [HttpGet("{id}")]
        public async Task<TacGia?> GetTacGiaAsync(string id)
        {
            var tg = await tacGiaService.GetTacGiaAsync(id);
            return tg;
        }

        [HttpPost]
        public async Task<ActionResult> AddTacGiaAsync(TacGia tg)
        {
            try
            {
                await tacGiaService.AddTacGiaAsync(tg);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTacGiaAsync(TacGia tg)
        {
            try
            {
                await tacGiaService.UpdateTacGiaAsync(tg);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTacGiaAsync(string id)
        {
            try
            {
                await tacGiaService.DeleteTacGiaAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
