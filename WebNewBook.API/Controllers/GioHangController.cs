﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebNewBook.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GioHangController : ControllerBase
	{
        private readonly IGioHangService _GioHangService;
        public GioHangController(IGioHangService GioHangService)
        {
            _GioHangService = GioHangService;
        }
      
        [HttpGet("ChecksoluongCart")]
        public async Task<int> ChecksoluongCart()
        {
            try
            {
                return await _GioHangService.ChecksoluongCart();
            }
            catch (Exception e)
            {
                return 0;
            }
           

        }
        [HttpGet("GetLitsGH")]
        public async Task<List<ModelCart>> GetGH()
        {

            return await _GioHangService.GetlistGH();

        }
      
        [HttpGet("GetSanPham/{id}")]
        public async Task<int> GetSanPham(string id)
        {
            return await _GioHangService.getSP(id);

        }
        [HttpGet("SanPham/{id}")]
        public async Task<SanPham> Getsp(string id)
        {
            return await _GioHangService.GetSanPham(id);

        }
        [HttpGet("Xoakhoigio/{id}/{namekh}")]
        public async Task<string> deleteCart(string id, string namekh)
        {
            try
            {
                await _GioHangService.XoakhoiGioHang(id, namekh);
                return "Thành Công";
            }
            catch (Exception e)
            {
                return "lỗi";
            }
           

        }
        [HttpGet("Updatenumber/{id}/{soluongmoi}/{namekh}/{update}")]
        public async Task<int> Updatenumber(string id,int soluongmoi,string namekh, string update)
        {

          
            try
            {
                return await _GioHangService.Updatenumber(id, soluongmoi, namekh, update);
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        [HttpGet("Addgiohang/{SoLuongs}/{emailKH}/{idsp}")]
        public async Task<int> Addgiohang(int SoLuongs,string emailKH,string idsp)
        {
            try
            {
                return await _GioHangService.AddGioHangAsync( SoLuongs, emailKH, idsp);
            }
            catch (Exception e)
            {
                return 0;
            }
           
           
        }
        [HttpPost("DeleteCarts/{email}")]
        public async Task<ActionResult> XoaGioHangKH(string email)
        {


            try
            {
                await _GioHangService.XoaGioHangKH(email);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
