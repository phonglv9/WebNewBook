﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class PhieuNhapController : Controller
    {
        private readonly IPhieuNhapService phieuNhapService;
        public PhieuNhapController(IPhieuNhapService phieuNhapService)
        {
            this.phieuNhapService = phieuNhapService;
        }

        [HttpGet]
        public async Task<IEnumerable<PhieuNhap>> GetPhieuNhapAsync()
        {
            var nv = await phieuNhapService.GetPhieuNhapAsync();
            return nv;
        }

        [HttpGet("{id}")]
        public async Task<PhieuNhap?> GetPhieuNhapAsync(string id)
        {
            var nv = await phieuNhapService.GetPhieuNhapAsync(id);
            return nv;
        }

        [HttpPost]
        public async Task<ActionResult> AddPhieuNhapAsync(PhieuNhap nv)
        {
            try
            {
                await phieuNhapService.AddPhieuNhapAsync(nv);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return Ok();
        }
    }
}
