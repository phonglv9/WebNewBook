using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _hoaDonService;

        public HoaDonController(IHoaDonService hoaDonService)
        {
            this._hoaDonService = hoaDonService;
        }
        [HttpPost("AddHoaDon")]
        public async Task<ActionResult> AddHoaDon(HoaDon hoaDon)
        {
            try
            {
                await _hoaDonService.AddHoaDon(hoaDon);
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost("AddHoaDonCT")]
        public async Task<ActionResult> AddHoaDonCT(List<HoaDonCT> hoaDonCTs)
        {
            try
            {
                await _hoaDonService.AddHoaDonCT(hoaDonCTs);
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpGet("UpdateTrangThai{id}")]
        public async Task<ActionResult> UpdateTrangThai(string id)
        {
            try
            {
                await _hoaDonService.UpdateTrangThai(id);

                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
