using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet("HomeVM")]
        public async Task<IEnumerable<HomeViewModel>> GetHomVMs()
        {
            return  await _homeService.GetHomVM();

        }
        [HttpGet("HomeProductTop10Oder")]
        public async Task<List<ProductOderTop10VM>> GetProductTop10Oder()
        {
            return await _homeService.GetTopProduct10Oder();

        }
        [HttpGet("Product")]
        public async Task<IEnumerable<ProductVM>> GetPrHomes()
        {
            return await _homeService.GetProductHome();

        }

        [HttpGet("ProductDetail/{id}")]
        public async Task<SanPhamChiTiet> ProductDetail(string id)
        {

            return await _homeService.GetProductDetail(id);
        }
        [HttpGet("TheLoai")]
        public async Task<List<TheLoai>> GetTL()
        {
            return await _homeService.GetTheLoais();

        }
        [HttpGet("TheLoaict/{id}")]
        public async Task<List<SanPhamChiTiet>> GetTLct(string id)
        {
            return await _homeService.GetTheLoaisCT(id);

        }
        [HttpGet("DanhMuc")]
        public async Task<List<DanhMucSach>> GetDM()
        {
            return await _homeService.GetDanhMucNavBar();

        }
        [HttpGet("TacGia")]
        public async Task<List<TacGia>> GetTG()
        {
            return await _homeService.GetTacGias();

        }



    }
}
