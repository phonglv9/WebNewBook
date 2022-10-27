using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;
using WebNewBook.Model.APIModels;

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
                if (item.SanPhamCTs.Select(c => c.MaSach).ToArray().Intersect(sachID).Count() == item.SanPhamCTs.Count() && item.SanPhamCTs.Count() == sachID.Count())
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
                sanPhamCT.MaSach = item.Substring(0, item.IndexOf("@") - 1);
                sanPhamCT.SoLuong = par.SoLuong;
                dbcontext.Add(sanPhamCT);

                Sach? sach = dbcontext.Sachs.FirstOrDefault(c => c.ID_Sach == item.Substring(0, item.IndexOf("@") - 1));
                sach.SoLuongKho -= par.SoLuong;
                if (sach.SoLuongKho < 0)
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

        public async Task<List<Sach>?> GetSachsBySanPhamAsync(string id)
        {
            var spcts = from sp in dbcontext.SanPhams.ToList()
                        join spct in dbcontext.SanPhamCTs.ToList() on sp.ID_SanPham equals spct.MaSanPham into spgroup
                        select new
                        {
                            SanPham = sp,
                            SanPhamCTs = spgroup
                        };
            var sachID = spcts.FirstOrDefault(c => c.SanPham.ID_SanPham == id).SanPhamCTs.Select(s => s.MaSach).ToList();
            List<Sach> sachs = new List<Sach>();
            dbcontext.Sachs.ToList().ForEach(s =>
            {
                if (sachID.Exists(c => c == s.ID_Sach))
                {
                    sachs.Add(s);
                }
            });

            return sachs;
        }

        public async Task UpdateSanPhamAsync(SanPham par, int slChuaDoi)
        {
            if (!dbcontext.SanPhams.AsNoTracking().ToList().Exists(c => c.TenSanPham == par.TenSanPham && c.ID_SanPham != par.ID_SanPham))
            {
                dbcontext.Update(par);
                if (slChuaDoi != par.SoLuong)
                {
                    int change = slChuaDoi - par.SoLuong;

                    List<Sach> sachs = new List<Sach>();
                    sachs = await GetSachsBySanPhamAsync(par.ID_SanPham);
                    foreach (var item in sachs)
                    {
                        Sach sach = dbcontext.Sachs.ToList().FirstOrDefault(c => c.ID_Sach == item.ID_Sach);
                        sach.SoLuongKho += change;

                        if (sach.SoLuongKho < 0)
                        {
                            throw new Exception("Hết sách rồi!");
                        }
                        dbcontext.Update(sach);
                    }
                }

                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task UpdateSanPhamAsync(SanPham par)
        {
            dbcontext.Update(par);
            await dbcontext.SaveChangesAsync();
        }
    }
}
