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
            par.MaDanhMuc = "DM638050907525310311";
            dbcontext.Add(par);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteTheLoaiAsync(string id)
        {
            TheLoai? tl = dbcontext.TheLoais.FirstOrDefault(c=>c.ID_TheLoai == id) ?? null;
            if (tl != null)
            {
                dbcontext.TheLoais.Remove(tl);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TheLoai>> GetTheLoaiAsync()
        {
            return await dbcontext.TheLoais.ToListAsync();
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
