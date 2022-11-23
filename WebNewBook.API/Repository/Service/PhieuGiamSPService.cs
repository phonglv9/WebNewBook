using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class PhieuGiamSPService : IPhieuGiamSPService
    {
        private readonly dbcontext dbcontext;

        public PhieuGiamSPService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddPhieuGiamGiaSPAsync(PhieuGiamGiaSP par)
        {
            dbcontext.Add(par);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeletePhieuGiamGiaSPAsync(string id)
        {
            PhieuGiamGiaSP? nv = dbcontext.PhieuGiamGiaSPs.FirstOrDefault(c => c.ID_PhieuGiamGiaSP == id) ?? null;
            if (nv != null)
            {
                dbcontext.PhieuGiamGiaSPs.Remove(nv);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PhieuGiamGiaSP>> GetPhieuGiamGiaSPAsync()
        {
            return await dbcontext.PhieuGiamGiaSPs.ToListAsync();
        }

        public async Task<PhieuGiamGiaSP?> GetPhieuGiamGiaSPAsync(string id)
        {
            return await dbcontext.PhieuGiamGiaSPs.FirstOrDefaultAsync(c => c.ID_PhieuGiamGiaSP == id) ?? null;
        }

        public async Task UpdatePhieuGiamGiaSPAsync(PhieuGiamGiaSP par)
        {
            dbcontext.Update(par);
            await dbcontext.SaveChangesAsync();
        }
    }
}
