using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
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
        [HttpGet("GetHD")]
        public async Task<List<ViewHoaDon>> getlist()
        {

            return await _hoaDonService.GetListHoaDon();
        }
        [HttpGet("getlistid/{id}")]
        public async Task<List<ViewHoaDonCT>> getlistid(string id)
        {

            return await _hoaDonService.GetHDCT(id);
        }
        [HttpGet("UpdateTT/{id}/{name}")]
        public async Task<ActionResult> UpdateTT(string id,int name,string? lydohuy)
        {
          
            

            try
            {
                _hoaDonService.UpdatetrangthaiHD(id, name,lydohuy);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //
        [HttpGet("GetPriceVoucher/{idvoucherCT}")]
        public async Task<Voucher> GetPriceVoucher( string idvoucherCT)
        {

            return await _hoaDonService.GetPriceVoucher(idvoucherCT);
        }

        [HttpPut("UpdateRecipientProfile")]
        public async Task<ActionResult> UpdateRecipientProfile(HoaDon hoaDon)
        {
            _hoaDonService.UpdateThongtinnguoinhan(hoaDon);
            return Ok();
        }

        [HttpPost("AddOrderAdmin")]
        public async Task<ActionResult> AddOrderAdminControll(HoaDon hoaDon)
        {
            _hoaDonService.AddOrderAdmin(hoaDon);
            return Ok();
        }
    }
}
