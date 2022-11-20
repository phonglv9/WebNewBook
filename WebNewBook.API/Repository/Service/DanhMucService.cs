using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class DanhMucService: IDanhMucService
    {
        private readonly dbcontext dbcontext;
        public DanhMucService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task AddDanhMucAsync(DanhMucSach danhMucSach)
        {
            dbcontext.Add(danhMucSach);
            await dbcontext.SaveChangesAsync();
        }

        public async Task DeleteDanhMucAsync(string id)
        {
            DanhMucSach? danhMucSachs = dbcontext.DanhMucSachs.FirstOrDefault(c => c.ID_DanhMuc == id) ?? null;
            if (danhMucSachs != null)
            {
                if (danhMucSachs.TrangThai == 1)
                {
                    danhMucSachs.TrangThai = 0;
                    dbcontext.DanhMucSachs.Update(danhMucSachs);
                    await dbcontext.SaveChangesAsync();
                }
                else
                {
                    danhMucSachs.TrangThai = 1;
                    dbcontext.DanhMucSachs.Update(danhMucSachs);
                    await dbcontext.SaveChangesAsync();
                }

            }
        }

        public async Task<IEnumerable<DanhMucSach>> GetDM()
        {
            return await dbcontext.DanhMucSachs.ToListAsync();
        }

        public async Task<DanhMucSach?> GetDanhMucAsync(string id)
        {
            return await dbcontext.DanhMucSachs.FirstOrDefaultAsync(c => c.ID_DanhMuc == id) ?? null;
        }

        public async Task UpdateDanhMucAsync(DanhMucSach danhMucSach)
        {
            dbcontext.Update(danhMucSach);
            await dbcontext.SaveChangesAsync();
        }
    }
}
