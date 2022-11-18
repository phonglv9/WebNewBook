using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaXuatBanController : Controller
    {
        private readonly INhaXuatBanService nhaXuatBanService;
        public NhaXuatBanController(INhaXuatBanService nhaXuatBanService)
        {
            this.nhaXuatBanService = nhaXuatBanService;
        }

        [HttpGet]
        public async Task<IEnumerable<NhaXuatBan>> GetNhaXuatBanAsync()
        {
            var nxb = await nhaXuatBanService.GetNhaXuatBanAsync();
            return nxb;
        }

        [HttpGet("{id}")]
        public async Task<NhaXuatBan?> GetNhaXuatBanAsync(string id)
        {
            var tl = await nhaXuatBanService.GetNhaXuatBanAsync(id);
            return tl;
        }

        [HttpPost]
        public async Task<ActionResult> AddNhaXuatBanAsync(NhaXuatBan nxb)
        {
            try
            {
                await nhaXuatBanService.AddNhaXuatBanAsync(nxb);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateNhaXuatBanAsync(NhaXuatBan nxb)
        {
            try
            {
                await nhaXuatBanService.UpdateNhaXuatBanAsync(nxb);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNhaXuatBanAsync(string id)
        {
            try
            {
                await nhaXuatBanService.DeleteNhaXuatBanAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
