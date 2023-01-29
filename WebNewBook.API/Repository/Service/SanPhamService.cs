using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;
using WebNewBook.Model.APIModels;

namespace WebNewBook.API.Repository.Service
{
    public class SanPhamService : ISanPhamService
    {
        private readonly dbcontext dbcontext;

        public SanPhamService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddSanPhamAsync(SanPhamAPI tsp)
        {
            var par = tsp.SanPham;
            IEnumerable<string> Sachs = tsp.Sachs;

            var spcts = from sp in dbcontext.SanPhams.ToList()
                        join spct in dbcontext.SanPhamCTs.ToList() on sp.ID_SanPham equals spct.MaSanPham into spgroup
                        select new
                        {
                            SanPham = sp,
                            SanPhamCTs = spgroup
                        };

            if (tsp.SLSachCT > 1 && Sachs.Count() > 1)
            {
                throw new Exception("Sản phẩm không hợp lệ!");
            }

            foreach (var item in spcts)
            {
                if ((tsp.SLSachCT == 1 && (item.SanPhamCTs.Where(c => c.SoLuongSach == 1).Select(c => c.MaSachCT).ToArray().Intersect(Sachs).Count() == item.SanPhamCTs.Count() && 
                    item.SanPhamCTs.Count() == Sachs.Count()))
                    || (tsp.SLSachCT > 1 && item.SanPhamCTs.Any(c => c.MaSachCT.Equals(Sachs.FirstOrDefault()) && c.SoLuongSach == tsp.SLSachCT)))
                {
                    throw new Exception("Đã tồn tại sản phẩm!");
                }
            }

            try
            {
                dbcontext.Add(par);
                foreach (var item in Sachs)
                {
                    SanPhamCT sanPhamCT = new SanPhamCT();
                    sanPhamCT.ID_SanPhamCT = Guid.NewGuid().ToString();
                    sanPhamCT.MaSanPham = par.ID_SanPham;
                    sanPhamCT.MaSachCT = item;
                    sanPhamCT.SoLuongSach =  tsp.SLSachCT;
                    dbcontext.Add(sanPhamCT);

                    SachCT? sach = dbcontext.SachCTs.FirstOrDefault(c => c.ID_SachCT == item);
                    sach.SoLuong -=  par.SoLuong * tsp.SLSachCT;
                    if (sach.SoLuong < 0)
                    {
                        throw new Exception("Hết sách rồi!");
                    }
                    dbcontext.Update(sach);
                }

                await dbcontext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Thêm sản phẩm thất bại!");
            }
        }

        public async Task<IEnumerable<SanPham>> GetSanPhamAsync()
        {
            return await dbcontext.SanPhams.ToListAsync();
        }

        public async Task<SanPham?> GetSanPhamAsync(string id)
        {
            return await dbcontext.SanPhams.FirstOrDefaultAsync(c => c.ID_SanPham == id) ?? null;
        }

        public IEnumerable<SanPhamViewModel> GetSanPhamViewModel()
        {
            var spcts = from sp in dbcontext.SanPhams.ToList()
                        join spct in dbcontext.SanPhamCTs.ToList() on sp.ID_SanPham equals spct.MaSanPham into spgroup
                        select new
                        {
                            SanPham = sp,
                            SanPhamCTs = spgroup
                        };

            var result = new List<SanPhamViewModel>();
            spcts.ToList().ForEach(c =>
            {
                var singleType = c.SanPhamCTs.Count() == 1;
                var sp = new SanPhamViewModel();
                sp.SanPham = c.SanPham;
                sp.TheLoaiSP = !singleType ? 2 : c.SanPhamCTs.FirstOrDefault().SoLuongSach == 1 ? 1 : 3;
                sp.SoLuongSach = c.SanPhamCTs.FirstOrDefault().SoLuongSach;
                //if (c.SanPhamCTs.Count() == 1)
                //{
                //    if (c.SanPhamCTs.FirstOrDefault().SoLuongSach == 1)
                //    {
                //        sp.TheLoaiSP = 1;
                //    }
                //    else
                //    {
                //        sp.TheLoaiSP = 3;
                //    }

                //}
                //else
                //{
                //    sp.TheLoaiSP = 2;
                //}

                result.Add(sp);
            });
            return result;
        }

        public async Task<List<SachCTViewModel>?> GetSachsBySanPhamAsync(string id)
        {
            //var spcts = from sp in dbcontext.SanPhams.ToList()
            //            join spct in dbcontext.SanPhamCTs.ToList() on sp.ID_SanPham equals spct.MaSanPham into spgroup
            //            select new
            //            {
            //                SanPham = sp,
            //                SanPhamCTs = spgroup
            //            };
            //var sachID = spcts.FirstOrDefault(c => c.SanPham.ID_SanPham == id).SanPhamCTs.Select(s => s.MaSachCT).ToList();
            //var sachID = dbcontext.SanPhamCTs.Where(c => c.MaSanPham == id).Select(s => s.MaSachCT).ToList();
            var result = dbcontext.SanPhams.Join(dbcontext.SanPhamCTs, sp => sp.ID_SanPham, spct => spct.MaSanPham, (sp, spct) => new { spct }).Where(c => c.spct.MaSanPham == id)
                .Join(dbcontext.SachCTs, sp => sp.spct.MaSachCT, sachCT => sachCT.ID_SachCT, (sp, sachCT) => new { sachCT }).Where(c => c.sachCT.TrangThai == 1).
                Join(dbcontext.Sachs, sachCT => sachCT.sachCT.MaSach, sach => sach.ID_Sach, (sachCT, sach) => new { sachCT.sachCT, sach.TenSach }).
                Join(dbcontext.NhaXuatBans, sachCT => sachCT.sachCT.MaNXB, nxb => nxb.ID_NXB, (sachCT, nxb) => new SachCTViewModel { SachCT = sachCT.sachCT, TenSach = sachCT.TenSach, NXB = nxb.TenXuatBan });
            //List<SachCT> sachs = new List<SachCT>();
            //dbcontext.SachCTs.Where(sct => sct.TrangThai == 1).ToList().ForEach(s =>
            //{
            //    if (sachID.Exists(c => c == s.ID_SachCT))
            //    {
            //        sachs.Add(s);
            //    }
            //});

            return result.ToList();
        }

        public async Task UpdateSanPhamAsync(SanPham par)
        {
            if (/*!dbcontext.SanPhams.ToList().Exists(c => c.TenSanPham == par.TenSanPham && c.ID_SanPham != par.ID_SanPham)*/true)
            {
                var spcts = dbcontext.SanPhamCTs.Where(c => c.MaSanPham.Equals(par.ID_SanPham)).ToList();
                dbcontext.Update(par);
                int sl = dbcontext.SanPhams.AsNoTracking<SanPham>().FirstOrDefault(c => c.ID_SanPham == par.ID_SanPham).SoLuong;
                if (sl != par.SoLuong)
                {
                    int change = sl - par.SoLuong;
                    if (spcts.Count == 1)
                    {
                        change = change * spcts.FirstOrDefault().SoLuongSach;
                    }
                    List<SachCT> sachs = new List<SachCT>();
                    var a = await GetSachsBySanPhamAsync(par.ID_SanPham);
                    sachs = a.Select(c => c.SachCT).ToList();


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
