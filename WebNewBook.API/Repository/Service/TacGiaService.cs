using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class TacGiaService : ITacGiaService
    {
        private readonly dbcontext dbcontext;
        public TacGiaService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task AddTacGiaAsync(TacGia par)
        {
            dbcontext.Add(par);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteTacGiaAsync(string id)
        {
            TacGia? tg = dbcontext.TacGias.FirstOrDefault(c => c.ID_TacGia == id) ?? null;
            if (tg != null)
            {
                dbcontext.TacGias.Remove(tg);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TacGia>> GetTacGiaAsync()
        {
            return await dbcontext.TacGias.ToListAsync();
        }

        public async Task<TacGia?> GetTacGiaAsync(string id)
        {
            return await dbcontext.TacGias.FirstOrDefaultAsync(c => c.ID_TacGia == id) ?? null;
        }

        public async Task UpdateTacGiaAsync(TacGia par)
        {
            dbcontext.Update(par);
            await dbcontext.SaveChangesAsync();
        }
    }
}
