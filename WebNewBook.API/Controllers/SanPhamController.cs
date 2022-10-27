using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;

namespace WebNewBook.API.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpGet("sanpham_sach/{id}")]
        public async Task<List<Sach>?> GetSachsBySanPhamAsync(string id)
        {
            var sachs = await sanPhamService.GetSachsBySanPhamAsync(id);
            return sachs;
        }

        [HttpPost]
        public async Task<ActionResult> AddSanPhamAsync(SanPhamAPI sp)
        {
            try
            {
                await sanPhamService.AddSanPhamAsync(sp.SanPham, sp.Sachs);
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSanPhamAsync(SanPhamAPI sp)
        {
            try
            {
                await sanPhamService.UpdateSanPhamAsync(sp.SanPham, sp.SLChuaDoi);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update_status")]
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
