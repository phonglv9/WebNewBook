using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonCTController : ControllerBase
    {
       private readonly IHoaDonCTService _hoaDonCTService;

        public HoaDonCTController(IHoaDonCTService hoaDonCTService)
        {
            _hoaDonCTService = hoaDonCTService;
        }
        [HttpPost("AddHoaDonCt")]
        public async Task<ActionResult> AddKHoaDonCTAsync(string mahd, string masp)
        {
            try
            {
                await _hoaDonCTService.AddOrderDetail(mahd,masp);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("UpdateHoaDonCT")]
        public async Task<ActionResult> UpdateHoaDonCTAsync(string mahdct , int soluong)
        {
            try
            {
                await _hoaDonCTService.UpdateOrderDetailQuantity(mahdct,soluong);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete("DeleteHoaDonCT/{mahdct}")]
        public async Task<ActionResult> DeleteHoaDonCTAsync(string mahdct)
        {
            try
            {
                await _hoaDonCTService.DeletaOrderDetail(mahdct);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
