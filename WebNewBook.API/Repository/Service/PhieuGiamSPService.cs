using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class PhieuGiamSPService 
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

        public async Task UpdatePhieuGiamGiaSPAsync(PhieuGiamGiaSP par)
        {
            dbcontext.Update(par);
            await dbcontext.SaveChangesAsync();
        }
    }
}
