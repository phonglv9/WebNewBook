using Microsoft.AspNetCore.Mvc;
using MimeKit;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PaymentController : ControllerBase
    {
        private readonly IHoaDonService _hoaDonService;

        public PaymentController(IHoaDonService hoaDonService)
        {
            this._hoaDonService = hoaDonService;
        }
        [HttpPost("GetHoaDon/{id}")]
        public async Task<HoaDon> GetHoaDon(string id)
        {
            var hoaDon = await _hoaDonService.GetHoaDon(id);
            return hoaDon;
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
        [HttpPost("UpdateTrangThai/{id}")]
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
        [HttpPost("UpdateSoLuongSP")]
        public async Task<ActionResult> UpdateSoLuongSanPham(List<HoaDonCT> hoaDonCTs)
        {
            try
            {
                await _hoaDonService.UpdateSLSanPham(hoaDonCTs);

                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost("UpdateSLVNPay/{id}")]
        public async Task<ActionResult> UpdateSLSanPhamVNPay(string id)
        {
            try
            {
                await _hoaDonService.UpdateSLSanPhamVNPay(id);

                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        [HttpPost("SendMailOder/{idHoaDon}")]
        public async Task<IActionResult> SendMailOder(string idHoaDon)
        {
            await _hoaDonService.SendMailOder(idHoaDon);

            return Ok();
        }

    }
}
