using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NhanVienController : Controller
    {
        private readonly INhanVienService nhanVienService;
        public NhanVienController(INhanVienService nhanVienService)
        {
            this.nhanVienService = nhanVienService;
        }

        [HttpGet]
        public async Task<IEnumerable<NhanVien>> GetNhanVienAsync()
        {
            var nv = await nhanVienService.GetNhanVienAsync();
            return nv;
        }

        [HttpGet("{id}")]
        public async Task<NhanVien?> GetNhanVienAsync(string id)
        {
            var nv = await nhanVienService.GetNhanVienAsync(id);
            return nv;

        }

        [HttpPost]
        public async Task<ActionResult> AddNhanVienAsync(NhanVien nv)
        {
            try
            {
                await nhanVienService.AddNhanVienAsync(nv);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateNhanVienAsync(NhanVien nv)
        {
            try
            {
                await nhanVienService.UpdateNhanVienAsync(nv);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNhanVienAsync(string id)
        {
            try
            {
                await nhanVienService.DeleteNhanVienAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
