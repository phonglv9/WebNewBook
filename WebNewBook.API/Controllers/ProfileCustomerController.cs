using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileCustomerController : ControllerBase
    {
        private readonly IProfileCustomerService _profileCustomerService;

        public ProfileCustomerController(IProfileCustomerService profileCustomerService)
        {
            _profileCustomerService = profileCustomerService;
        }
        /// <summary>
        /// lấy tất cả hóa đơn của khách hàng trong năm hiện tại
        /// </summary>
        /// <param name="makhachhang"></param>
        /// <returns></returns>
        [HttpGet("GetOrder/{makhachhang}")]
        public async Task<IEnumerable< HoaDon>> GetOrdercustomerrAsync(string makhachhang)
        {
            var model = await _profileCustomerService.GetOrdersAsync(makhachhang);
            return model;
        }
        /// <summary>
        /// lấy hóa đơn chi tiêt
        /// </summary>
        /// <param name="mahoadon"></param>
        /// <returns></returns>
        [HttpGet("GetOrderdetail/{mahoadon}")]
        public async Task<List<HoaDonCT>> GetOrderDetailAsync(string mahoadon)
        {
            var model = await _profileCustomerService.GetOrderDetailAsync(mahoadon);
            return model;
        }
        /// <summary>
        /// Gọi Order theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetOrderById/{id}")]
        public async Task<HoaDon> GetOrderById(string id)
        {
            var model = await _profileCustomerService.GetOrderByIdAsync(id);
            return model;
        }
        [HttpGet("GetListOrder/{mahoadon}")]
        public async Task<List<ViewHoaDon>> GetListOrderDetail(string mahoadon)
        {
            var model = await _profileCustomerService.GetListOrder(mahoadon);
            return model;
        } 
    }
}
