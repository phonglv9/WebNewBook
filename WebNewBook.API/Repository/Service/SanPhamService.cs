﻿using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;

namespace WebNewBook.API.Repository.Service
{
    public class SanPhamService : ISanPhamService
    {
        private readonly dbcontext dbcontext;

        public SanPhamService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddSanPhamAsync(SanPham par, IEnumerable<string> Sachs)
        {
            var spcts = from sp in dbcontext.SanPhams.ToList()
                        join spct in dbcontext.SanPhamCTs.ToList() on sp.ID_SanPham equals spct.MaSanPham into spgroup
                        select new
                        {
                            SanPham = sp,
                            SanPhamCTs = spgroup
                        };

            var sachID = Sachs.Select(c => c.Substring(0, c.IndexOf("@") - 1));
            foreach (var item in spcts)
            {
                if ((item.SanPhamCTs.Select(c => c.MaSachCT).ToArray().Intersect(sachID).Count() == item.SanPhamCTs.Count() && item.SanPhamCTs.Count() == sachID.Count()) || dbcontext.SanPhams.ToList().Exists(c => c.TenSanPham == par.TenSanPham))
                {
                    throw new Exception("Đã tồn tại sản phẩm!");
                }
            }

            dbcontext.Add(par);
            foreach (var item in Sachs)
            {
                SanPhamCT sanPhamCT = new SanPhamCT();
                sanPhamCT.ID_SanPhamCT = Guid.NewGuid().ToString();
                sanPhamCT.MaSanPham = par.ID_SanPham;
                sanPhamCT.MaSachCT = item.Substring(0, item.IndexOf("@") - 1);
                dbcontext.Add(sanPhamCT);

                SachCT? sach = dbcontext.SachCTs.FirstOrDefault(c => c.ID_SachCT == item.Substring(0, item.IndexOf("@") - 1));
                sach.SoLuong -= par.SoLuong;
                if (sach.SoLuong < 0)
                {
                    throw new Exception("Hết sách rồi!");
                }
                dbcontext.Update(sach);
            }
            await dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<SanPham>> GetSanPhamAsync()
        {
            return await dbcontext.SanPhams.ToListAsync();
        }

        public async Task<SanPham?> GetSanPhamAsync(string id)
        {
            return await dbcontext.SanPhams.FirstOrDefaultAsync(c => c.ID_SanPham == id) ?? null;
        }

        public async Task<List<SachCT>?> GetSachsBySanPhamAsync(string id)
        {
            //var spcts = from sp in dbcontext.SanPhams.ToList()
            //            join spct in dbcontext.SanPhamCTs.ToList() on sp.ID_SanPham equals spct.MaSanPham into spgroup
            //            select new
            //            {
            //                SanPham = sp,
            //                SanPhamCTs = spgroup
            //            };
            //var sachID = spcts.FirstOrDefault(c => c.SanPham.ID_SanPham == id).SanPhamCTs.Select(s => s.MaSachCT).ToList();
            var sachID = dbcontext.SanPhamCTs.Where(c => c.MaSanPham == id).Select(s => s.MaSachCT).ToList();
            List<SachCT> sachs = new List<SachCT>();
            dbcontext.SachCTs.Where(sct => sct.TrangThai == 1).ToList().ForEach(s =>
            {
                if (sachID.Exists(c => c == s.ID_SachCT))
                {
                    sachs.Add(s);
                }
            });

            return sachs;
        }

        public async Task UpdateSanPhamAsync(SanPham par)
        {
            if (/*!dbcontext.SanPhams.ToList().Exists(c => c.TenSanPham == par.TenSanPham && c.ID_SanPham != par.ID_SanPham)*/true)
            {
                dbcontext.Update(par);
                int sl = dbcontext.SanPhams.AsNoTracking<SanPham>().FirstOrDefault(c => c.ID_SanPham == par.ID_SanPham).SoLuong;
                if (sl != par.SoLuong)
                {
                    int change = sl - par.SoLuong;

                    List<SachCT> sachs = new List<SachCT>();
                    sachs = await GetSachsBySanPhamAsync(par.ID_SanPham);
                    foreach (var item in sachs)
                    {
                        SachCT sach = dbcontext.SachCTs.ToList().FirstOrDefault(c => c.ID_SachCT == item.ID_SachCT);
                        sach.SoLuong += change;

                        if (sach.SoLuong < 0)
                        {
                            throw new Exception("Hết sách rồi!");
                        }
                        dbcontext.Update(sach);
                    }
                }
            }
            await dbcontext.SaveChangesAsync();
        }
    }
}
