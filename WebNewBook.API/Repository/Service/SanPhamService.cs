using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;

namespace WebNewBook.API.Repository.Service
{
    public class SanPhamService: ISanPhamService
    {
        private readonly dbcontext dbcontext;

        public SanPhamService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddSanPhamAsync(SanPham par, IEnumerable<string> Sachs)
        {
            dbcontext.Add(par);
            foreach (var item in Sachs)
            {
                SanPhamCT sanPhamCT = new SanPhamCT();
                sanPhamCT.ID_SanPhamCT = Guid.NewGuid().ToString();
                sanPhamCT.MaSanPham = par.ID_SanPham;
                sanPhamCT.MaSach = item;
                sanPhamCT.SoLuong = par.SoLuong;
                dbcontext.Add(sanPhamCT);

                Sach? sach = dbcontext.Sachs.FirstOrDefault(c => c.ID_Sach == item);
                sach.SoLuongKho -= par.SoLuong;
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

        public async Task UpdateSanPhamAsync(SanPham par)
        {
            if (!dbcontext.SanPhams.ToList().Exists(c => c.TenSanPham == par.TenSanPham && c.ID_SanPham != par.ID_SanPham))
            {
                dbcontext.Update(par);
                int sl = dbcontext.SanPhamCTs.FirstOrDefault(c => c.MaSanPham == par.ID_SanPham).SoLuong;
                if (sl != par.SoLuong)
                {
                    int change = sl - par.SoLuong;
                    foreach (var item in dbcontext.SanPhamCTs.Where(c => c.MaSanPham == par.ID_SanPham))
                    {
                        item.SoLuong = par.SoLuong;
                        dbcontext.Update(item);

                        Sach? sach = dbcontext.Sachs.FirstOrDefault(c => c.ID_Sach == item.MaSach);
                        sach.SoLuong += sl;
                        dbcontext.Update(sach);
                    }
                }
            }
            await dbcontext.SaveChangesAsync();
        }
    }
}
