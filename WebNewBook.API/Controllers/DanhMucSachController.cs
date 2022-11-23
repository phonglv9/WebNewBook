using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucSachController : ControllerBase
    {
        private readonly IDanhMucService _danhMucService;
        public DanhMucSachController(IDanhMucService danhMucService)
        {
            this._danhMucService = danhMucService;
        }

        [HttpGet]
        public async Task<IEnumerable<DanhMucSach>> GetDMAsync()
        {
            var nxb = await _danhMucService.GetDM();
            return nxb;
        }

        [HttpGet("{id}")]
        public async Task<DanhMucSach?> GetNhaXuatBanAsync(string id)
        {
            var tl = await _danhMucService.GetDanhMucAsync(id);
            return tl;
        }

        [HttpPost]
        public async Task<ActionResult> AddDanhMucAsync(DanhMucSach danhMuc)
        {
            try
            {
                await _danhMucService.AddDanhMucAsync(danhMuc);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateDanhMucAsync(DanhMucSach danhMucSach)
        {
            try
            {
                await _danhMucService.UpdateDanhMucAsync(danhMucSach);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> DeleteNhaXuatBanAsync(string id)
        {
            try
            {
                await _danhMucService.DeleteDanhMucAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
