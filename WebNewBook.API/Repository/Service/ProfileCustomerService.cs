﻿using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
namespace WebNewBook.API.Repository.Service
{
    public class ProfileCustomerService : IProfileCustomerService
    {
        private readonly dbcontext _dbcontext;

        public ProfileCustomerService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<HoaDon?> GetOrderByIdAsync(string id)
        {
            try
            {
                return await _dbcontext.HoaDons.FirstOrDefaultAsync(c => c.ID_HoaDon == id) ?? null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<HoaDonCT>> GetOrderDetailAsync(string mahoadon)
        {
            try
            {
             var model=  await _dbcontext.HoaDonCTs.Where(c => c.MaHoaDon == mahoadon).ToListAsync();
                return model;
            }
            catch (Exception)
            {

                throw;
            };
        }

        public async Task<List<HoaDon>> GetOrdersAsync(string makhachhang)
        {
            try
            {
                var model = await _dbcontext.HoaDons.Where(c => c.MaKhachHang == makhachhang ).ToListAsync();

                return model;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}