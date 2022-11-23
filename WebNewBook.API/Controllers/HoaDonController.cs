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
        [HttpGet("{id}")]
        public async Task<ViewHoaDonCT> getlistid(string id)
        {

            return await _hoaDonService.GetListid(id);
        }
        [HttpGet("UpdateTT/{id}/{name}")]
        public async Task<HoaDon?> UpdateTT(string id,int name)
        {
            HoaDon b=new HoaDon();
            var a= _hoaDonService.Updatetrangthai(id, name);
            return  b;
        }

    }
}
