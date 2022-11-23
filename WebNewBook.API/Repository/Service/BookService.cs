using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class BookService : IBookSevice
    {
        private readonly dbcontext _dbcontext;
        public BookService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task CreateBook(SachAPI input)
        {
            if (_dbcontext.Sachs.ToList().Exists(c => c.TenSach == input.Sach.TenSach))
            {
                throw new Exception("Đã tồn tại tên sách!");
            }
            _dbcontext.Add(input.Sach);

            foreach (var tacGia in input.TacGias)
            {
                foreach (var theLoai in input.TheLoais)
                {
                    var id = "SachCT" + Guid.NewGuid().ToString();
                    SachCT sachCT = new SachCT { ID_SachCT = id, MaSach = input.Sach.ID_Sach, MaTacGia = tacGia, MaTheLoai = theLoai };
                    _dbcontext.Add(sachCT);
                }
            }

            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeteleBook(string ID)
        {
            try
            {
                var Model = _dbcontext.Sachs.FirstOrDefault(c => c.ID_Sach == ID);
                if (Model != null)
                {
                    if (Model.TrangThai == 0)
                    {
                        Model.TrangThai = 1;
                    }
                    else
                    {
                        Model.TrangThai = 0;
                    }
                    _dbcontext.Update(Model);
                    await _dbcontext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Đổi trạng thái thất bại!");
            }
        }

        public async Task<IEnumerable<Sach>> GetListBook()
        {
            try
            {
                return await _dbcontext.Sachs.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateBook(SachAPI input)
        {
            try
            {
                if (_dbcontext.Sachs.AsNoTracking().ToList().Exists(c => c.ID_Sach != input.Sach.ID_Sach && c.TenSach == input.Sach.TenSach))
                {
                    throw new Exception("Tên đã tồn tại!");
                }
                
                _dbcontext.Update(input.Sach);
                var sachCTs = new List<SachCT>();
                sachCTs = await GetSachCT(input.Sach.ID_Sach);
                foreach (var tacGia in input.TacGias)
                {
                    foreach (var theLoai in input.TheLoais)
                    {
                        if (!sachCTs.Exists(c => c.MaTacGia == tacGia && c.MaTheLoai == theLoai))
                        {
                            SachCT sachCT = new SachCT { ID_SachCT = "SachCT" + Guid.NewGuid().ToString(), MaSach = input.Sach.ID_Sach, MaTacGia = tacGia, MaTheLoai = theLoai };
                            _dbcontext.Add(sachCT);
                        }
                    }
                }

                foreach (var sachCT in sachCTs)
                {
                    if (!input.TacGias.Contains(sachCT.MaTacGia) || !input.TheLoais.Contains(sachCT.MaTheLoai))
                    {
                        _dbcontext.Remove(sachCT);
                    }
                }

                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Cập nhật thất bại!");
            }
        }

        public async Task<List<SachCT>> GetSachCT(string id)
        {
            return await _dbcontext.SachCTs.Where(c => c.MaSach == id).ToListAsync();
        }
    }
}
