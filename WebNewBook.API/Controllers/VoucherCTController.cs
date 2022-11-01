﻿using Microsoft.AspNetCore.Http;
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
        /// <summary>
        /// Lấy danh sách voucher
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<VoucherCT>> GetVoucherAsync()
        {
            var voucher = await _voucherCTServices.GetVoucherChuaphathanhAsync();
    
            return voucher;
        }
        /// <summary>
        /// Lấy voucher theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<VoucherCT?> GetVoucherByIdAsync(string id)
        {
            var voucher = await _voucherCTServices.GetVoucherByIdAsync(id);
            return voucher;
        }

        /// <summary>
        /// lấy Voucher theo phát hành
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("CallIdPH/{id}")]
        public async Task<IEnumerable<VoucherCT?>> GetVoucherBymavoucherAsync(string id)
        {
            var voucher = await _voucherCTServices.GetVoucherByMaVoucherAsync(id);
            return voucher;
        }
        /// <summary>
        /// Thêm từng voucher thủ công
        /// </summary>
        /// <param name="voucherCT"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Thêm danh sách voucher tự động 
        /// </summary>
        /// <param name="quantityVoucher"></param>
        /// <param name="sizeVoucher"></param>
        /// <param name="startTextVoucher"></param>
        /// <param name="endTextVoucher"></param>
        /// <param name="maVoucher"></param>
        /// <returns></returns>
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

        /// <summary>
        /// thêm voucher bằng file excel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="Phathanh"></param>
        /// <returns></returns>
        [HttpPost("AddImportExcer")]
        public async Task<ActionResult> AddImportExcerVoucher(IFormFile file, string Phathanh)
        {
            try
            {
                await _voucherCTServices.AddImportExcerAsync(file,Phathanh);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }

        /// <summary>
        /// lấy Voucher của khách hàng
        /// </summary>
        /// <param name="maKhachHang"></param>
        /// <returns></returns>
        [HttpGet("VoucherKH/{maKhachHang}")]
        public async Task<IEnumerable<VoucherCT?>> GetVoucherOfCustomer(string maKhachHang)
        {
            var voucher = await _voucherCTServices.GetVoucherOfCustomer(maKhachHang);
            return voucher;
        }
    }
}