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
        private readonly IVoucherService _voucher;

        public VouCherController(IVoucherService voucher)
        {
            _voucher = voucher;
        }
        [HttpGet]
        public async Task<IEnumerable<Voucher>> GetVoucherAsync()
        {
            var voucher = await _voucher.GetVouCherAsync();
            return voucher;
        }

        [HttpGet("{id}")]
        public async Task<Voucher?> GetVoucherByIdAsync(string id)
        {
            var voucher = await _voucher.GetVouCherByIdAsync(id);
            return voucher;
        }

        [HttpPost]
        public async Task<ActionResult> AddVoucherAsync(Voucher voucher)
        {
            try
            {
                await _voucher.AddVouCherAsync(voucher);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateVoucherAsync(Voucher voucher)
        {
            try
            {
                await _voucher.UpdateVouCherAsync(voucher);
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
