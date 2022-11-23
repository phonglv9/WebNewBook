using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GioHangController : ControllerBase
	{
        private readonly IGioHangService _GioHangService;
        public GioHangController(IGioHangService GioHangService)
        {
            _GioHangService = GioHangService;
        }
        [HttpGet("GioHangVM")]
        public async Task<List<HomeVM>> GetHomVMs()
        {
            return await _GioHangService.VM();

        }
        [HttpGet("GetLitsGH")]
        public async Task<List<GioHang>> GetGH()
        {
            return await _GioHangService.GetlistGH();

        }
        [HttpGet("SanPham/{id}")]
        public async Task<SanPham> Getsp(string id)
        {
            return await _GioHangService.GetSanPham(id);

        }
        [HttpGet("Xoakhoigio/{id}")]
        public async Task<string> deleteCart(string id)
        {
             await _GioHangService.XoakhoiGioHang(id);
            return "Thành Công";

        }
        [HttpGet("Addgiohang/{HinhAnh}/{SoLuongs}/{emailKH}/{idsp}")]
        public async Task<ActionResult> Addgiohang(string HinhAnh,int SoLuongs,string emailKH,string idsp)
        {
            
          
            try
            {
                await _GioHangService.AddGioHangAsync(HinhAnh, SoLuongs, emailKH, idsp);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
       
    }
}
