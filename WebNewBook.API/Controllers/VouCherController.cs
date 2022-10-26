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
        /// <summary>
        /// Lấy danh sách phát hành Voucher
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Voucher>> GetVoucherAsync()
        {
            var voucher = await _voucher.GetVouCherAsync();
            return voucher;
        }
        /// <summary>
        /// Lấy Đối tượng phát hành theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Voucher?> GetVoucherByIdAsync(string id)
        {
            var voucher = await _voucher.GetVouCherByIdAsync(id);
            return voucher;
        }
        /// <summary>
        /// Thêm  phát hành
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Sửa phát hành
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Xóa phát hành
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
