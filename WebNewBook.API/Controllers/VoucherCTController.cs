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
        [HttpGet]
        public async Task<IEnumerable<VoucherCT>> GetVoucherAsync()
        {
            var voucher = await _voucherCTServices.GetVoucherChuaphathanhAsync();
    
            return voucher;
        }

        [HttpGet("{id}")]
        public async Task<VoucherCT?> GetVoucherByIdAsync(string id)
        {
            var voucher = await _voucherCTServices.GetVoucherByIdAsync(id);
            return voucher;
        }


        [HttpGet("CallIdPH/{id}")]
        public async Task<IEnumerable<VoucherCT?>> GetVoucherBymavoucherAsync(string id)
        {
            var voucher = await _voucherCTServices.GetVoucherByMaVoucherAsync(id);
            return voucher;
        }

        [HttpPost("AddManually")]
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
        [HttpPost("AddAutomatically")]
        public async Task<ActionResult> AddTuDongVoucher(int quantityVoucher, int sizeVoucher, string startTextVoucher, string endTextVoucher, string maVoucher)
        {
            try
            {
                await _voucherCTServices.AddAutomaticallyAsync(quantityVoucher, sizeVoucher, startTextVoucher, endTextVoucher,maVoucher);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
                
            }
        }
    }
}
