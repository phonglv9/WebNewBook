using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class NhaXuatBanService: INhaXuatBanService
    {
        private readonly dbcontext dbcontext;
        public NhaXuatBanService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task AddNhaXuatBanAsync(NhaXuatBan par)
        {
            dbcontext.Add(par);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteNhaXuatBanAsync(string id)
        {
            NhaXuatBan? nxb = dbcontext.NhaXuatBans.FirstOrDefault(c => c.ID_NXB == id) ?? null;
            if (nxb != null)
            {
                dbcontext.NhaXuatBans.Remove(nxb);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NhaXuatBan>> GetNhaXuatBanAsync()
        {
            return await dbcontext.NhaXuatBans.ToListAsync();
        }

        public async Task<NhaXuatBan?> GetNhaXuatBanAsync(string id)
        {
            return await dbcontext.NhaXuatBans.FirstOrDefaultAsync(c => c.ID_NXB == id) ?? null;
        }

        public async Task UpdateNhaXuatBanAsync(NhaXuatBan par)
        {
            dbcontext.Update(par);
            await dbcontext.SaveChangesAsync();
        }
    }
}
