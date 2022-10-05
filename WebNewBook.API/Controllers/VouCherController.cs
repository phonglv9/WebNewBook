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
    public class VouCherController : ControllerBase
    {
        private readonly IVoucher _voucher;

        public VouCherController(IVoucher voucher)
        {
            _voucher = voucher;
        }
        [HttpGet]
        public async Task<IEnumerable<VoucherCT>> GetVoucherAsync()
        {
            var voucher = await _voucher.GetVouCherAsync();
            return voucher;
        }

        [HttpGet("{id}")]
        public async Task<VoucherCT?> GetVoucherByIdAsync(string id)
        {
            var voucher = await _voucher.GetVouCherByIdAsync(id);
            return voucher;
        }

        [HttpPost]
        public async Task<ActionResult> AddVoucherAsync(VoucherCT phieuGiamGia)
        {
            try
            {
                await _voucher.AddVouCherAsync(phieuGiamGia);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateVoucherAsync(VoucherCT phieuGiamGia)
        {
            try
            {
                await _voucher.UpdateVouCherAsync(phieuGiamGia);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DeleteVoucherAsync(string id)
        {
            try
            {
                await _voucher.DeleteVouCherAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
