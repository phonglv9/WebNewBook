﻿using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;

namespace WebNewBook.API.Repository.Service
{
    public class PhieuNhapService : IPhieuNhapService
    {
        private readonly dbcontext dbcontext;

        public PhieuNhapService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddPhieuNhapAsync(PhieuNhap par)
        {
            par.NgayNhap = DateTime.Now;
            dbcontext.Add(par);
            SachCT sach = dbcontext.SachCTs.FirstOrDefault(c => c.ID_SachCT == par.MaSachCT);
            sach.SoLuong += par.SoLuongNhap;
            dbcontext.Update(sach);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeletePhieuNhapAsync(string id)
        {
            PhieuNhap? nv = dbcontext.PhieuNhaps.FirstOrDefault(c => c.ID_PhieuNhap == id) ?? null;
            if (nv != null)
            {
                dbcontext.PhieuNhaps.Remove(nv);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PhieuNhap>> GetPhieuNhapAsync()
        {
            return await dbcontext.PhieuNhaps.ToListAsync();
        }

        public async Task<PhieuNhap?> GetPhieuNhapAsync(string id)
        {
            return await dbcontext.PhieuNhaps.FirstOrDefaultAsync(c => c.ID_PhieuNhap == id) ?? null;
        }

        public async Task UpdatePhieuNhapAsync(PhieuNhap par)
        {
            dbcontext.Update(par);
            await dbcontext.SaveChangesAsync();
        }
    }
}
