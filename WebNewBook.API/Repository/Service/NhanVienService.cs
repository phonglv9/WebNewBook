using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;

namespace WebNewBook.API.Repository.Service
{
    public class NhanVienService : INhanVienService
    {
        private readonly dbcontext dbcontext;

        public NhanVienService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task AddNhanVienAsync(NhanVien par)
        {
            dbcontext.Add(par);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteNhanVienAsync(string id)
        {
            NhanVien? nv = dbcontext.NhanViens.FirstOrDefault(c => c.ID_NhanVien == id) ?? null;
            if (nv != null)
            {
                dbcontext.NhanViens.Remove(nv);
                await dbcontext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NhanVien>> GetNhanVienAsync()
        {
            return await dbcontext.NhanViens.ToListAsync();
        }

        public async Task<NhanVien?> GetNhanVienAsync(string id)
        {
            return await dbcontext.NhanViens.FirstOrDefaultAsync(c => c.ID_NhanVien == id) ?? null;
        }

        public async Task UpdateNhanVienAsync(NhanVien par)
        {
            dbcontext.Update(par);
            await dbcontext.SaveChangesAsync();
        }
    }
}
