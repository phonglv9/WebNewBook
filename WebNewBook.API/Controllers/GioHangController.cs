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
        [HttpGet("SanPham/{id}")]
        public async Task<SanPham> Getsp(string id)
        {
            return await _GioHangService.GetSanPham(id);

        }
    }
}
