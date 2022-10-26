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
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <param name="search"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<KhachHang>> GetKkachHangAsync(string? search, int? status)
        {
            var custmer = await _customerService.GetKhachHangsAsync(search, status);
            return custmer;
        }
        /// <summary>
        /// Lấy khách hàng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<KhachHang?> GetKhachHangByIdAsync(string id)
        {
            var customer = await _customerService.GetKhachHangByIdAsync(id);
            return customer;
        }
        /// <summary>
        /// Thêm khách hàng
        /// </summary>
        /// <param name="khachHang"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AddKhachHangAsync(KhachHang khachHang)
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
        /// <summary>
        /// sửa khách hàng
        /// </summary>
        /// <param name="khachHang"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> UpdateKhachHangAsync(KhachHang khachHang)
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
        /// <summary>
        /// Xóa khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> DeleteKhachHangAsync(string id)
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
