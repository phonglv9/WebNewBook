﻿using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;

namespace WebNewBook.API.Repository.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly dbcontext _dbcontext;

        public CustomerService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddKhachHangAsync(KhachHang khachHang)
        {
            _dbcontext.Add(khachHang);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteKhachHangAsync(string id)
        {
            KhachHang? khachhang = _dbcontext.KhachHangs.FirstOrDefault(c => c.ID_KhachHang == id) ?? null;
            if (khachhang != null)
            {
                khachhang.TrangThai = 0;
                _dbcontext.KhachHangs.Update(khachhang);
                await _dbcontext.SaveChangesAsync();
            }
        }

        public async Task<KhachHang?> GetKhachHangByIdAsync(string id)
        {
            return await _dbcontext.KhachHangs.FirstOrDefaultAsync(c => c.ID_KhachHang == id) ?? null;
        }

        public async Task<IEnumerable<KhachHang>> GetKhachHangsAsync()
        {
            return await _dbcontext.KhachHangs.ToListAsync();
        }

        public async Task UpdateKhachHangAsync(KhachHang khachHang)
        {
            _dbcontext.Update(khachHang);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
