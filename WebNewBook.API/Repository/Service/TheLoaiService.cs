using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class TheLoaiService : ITheLoaiService
    {
        private readonly dbcontext dbcontext;
        public TheLoaiService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddTheLoaiAsync(TheLoai par)
        {
            
            dbcontext.Add(par);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteTheLoaiAsync(string id)
        {
            TheLoai? tl = dbcontext.TheLoais.FirstOrDefault(c => c.ID_TheLoai == id) ?? null;
            if (tl != null)
            {
                if (tl.TrangThai == 1)
                {
                    tl.TrangThai = 0;
                    dbcontext.TheLoais.Update(tl);
                    await dbcontext.SaveChangesAsync();
                }
                else
                {
                    tl.TrangThai = 1;
                    dbcontext.TheLoais.Update(tl);
                    await dbcontext.SaveChangesAsync();
                }
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TheLoai>> GetTheLoaiAsync()
        {
            return await dbcontext.TheLoais.Include(c=>c.DanhMucSach).ToListAsync();
        }

        public async Task<TheLoai?> GetTheLoaiAsync(string id)
        {
            return await dbcontext.TheLoais.FirstOrDefaultAsync(c => c.ID_TheLoai == id) ?? null;
        }

        public async Task UpdateTheLoaiAsync(TheLoai par)
        {
            dbcontext.Update(par);
            await dbcontext.SaveChangesAsync();
        }
    }
}
