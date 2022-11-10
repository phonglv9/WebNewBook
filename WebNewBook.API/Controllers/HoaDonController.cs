using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;

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

    }
}
