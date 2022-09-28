using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IEnumerable<KhachHang>> GetNhanVienAsync()
        {
            var custmer = await _customerService.GetKhachHangsAsync();
            return custmer;
        }

        [HttpGet("{id}")]
        public async Task<KhachHang?> GetKhachHangByIdAsync(string id)
        {
            var customer = await _customerService.GetKhachHangByIdAsync(id);
            return customer;
        }

        [HttpPost]
        public async Task<ActionResult> AddNhanVienAsync(KhachHang khachHang)
        {
            try
            {
                await _customerService.AddKhachHangAsync(khachHang);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateNhanVienAsync(KhachHang khachHang)
        {
            try
            {
                await _customerService.UpdateKhachHangAsync(khachHang);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNhanVienAsync(string id)
        {
            try
            {
                await _customerService.DeleteKhachHangAsync(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
