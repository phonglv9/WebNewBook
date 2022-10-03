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
        public async Task<List<HomeVM>> GetHomVMs()
        {
            return  await _homeService.GetHomVM();

        }
        [HttpGet]
        public async Task<List<HomeVM>> GetProuctHomes(string? search, string? iddanhmuc)
        {
            return await _homeService.GetProductHome(search,iddanhmuc);

        }
        [HttpGet("TheLoai")]
        public async Task<List<TheLoai>> GetTL()
        {
            return await _homeService.GetTheLoais();

        }
        [HttpGet("DanhMuc")]
        public async Task<List<DanhMucSach>> GetDM()
        {
            return await _homeService.GetDanhMucs();

        }



    }
}
