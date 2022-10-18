using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherCTController : ControllerBase
    {
        private readonly IVoucherCTServices _voucherCTServices;

        public VoucherCTController(IVoucherCTServices voucherCTServices)
        {
            _voucherCTServices = voucherCTServices;
        }


        [HttpPost]
        public async Task<ActionResult> AddthucongVoucherAsync(VoucherCT voucherCT)
        {
            try
            {
                await _voucherCTServices.AddManuallyAsync(voucherCT);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
