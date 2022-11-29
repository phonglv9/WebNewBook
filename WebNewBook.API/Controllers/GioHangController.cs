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
        [HttpGet("Xoakhoigio/{id}/{namekh}")]
        public async Task<string> deleteCart(string id, string namekh)
        {
             await _GioHangService.XoakhoiGioHang(id, namekh);
            return "Thành Công";

        }
        [HttpGet("Updatenumber/{id}/{soluongmoi}/{namekh}/{update}")]
        public async Task<int> Updatenumber(string id,int soluongmoi,string namekh, string update)
        {

            return await _GioHangService.Updatenumber(id, soluongmoi, namekh,update); 

        }
        [HttpGet("Addgiohang/{HinhAnh}/{SoLuongs}/{emailKH}/{idsp}")]
        public async Task<int> Addgiohang(string HinhAnh,int SoLuongs,string emailKH,string idsp)
        {
            
          
            
            
                
                return await _GioHangService.AddGioHangAsync(HinhAnh, SoLuongs, emailKH, idsp); 
           
        }
       
    }
}
