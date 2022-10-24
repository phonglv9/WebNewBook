using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WebNewBook.API.Book;
using WebNewBook.API.Data;
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

        public async Task<Sach> CreateBook(CreateBookModel input)
        {
            try
            {
                if (String.IsNullOrEmpty(input.TenSach) || String.IsNullOrEmpty(input.HinhAnh) || input.SoTrang < 20 || input.GiaBan == 0 || input.SoLuong == 0 || input.SoLuongKho < input.SoLuong)
                {
                    return null;
                }
                var model = new Sach
                {
                    ID_Sach = "S" + _dbcontext.Sachs.Count(),
                    TenSach = input.TenSach,
                    HinhAnh = input.HinhAnh,
                    SoTrang = input.SoTrang,
                    SoLuong = input.SoLuong,
                    SoLuongKho = input.SoLuongKho,
                    TaiBan = input.TaiBan,
                    GiaBan = input.GiaBan,
                    MaNXB = input.MaNXB,
                    MoTa = input.MoTa,
                    TrangThai = 1,
                };
                await _dbcontext.AddAsync(model);

                if (input.ListTacGia != null)
                {
                    foreach (var tg in input.ListTacGia)
                    {
                        var TacGia = await _dbcontext.TacGias.Where(c => c.ID_TacGia == tg).FirstOrDefaultAsync();
                        if (TacGia == null)
                        {
                            return null;
                        }
                        foreach (var tl in input.ListTheLoai)
                        {
                            var TheLoai = await _dbcontext.TheLoais.Where(c => c.ID_TheLoai == tl).FirstOrDefaultAsync();
                            if (TheLoai == null)
                            {
                                return null;
                            }
                            var AuthorBook = new SachCT
                            {
                                ID_SachCT = Guid.NewGuid().ToString(),
                                MaTacGia = tg,
                                MaTheLoai = tl,
                                MaSach = model.ID_Sach

                            };
                            await _dbcontext.AddAsync(AuthorBook);
                            await _dbcontext.SaveChangesAsync();
                        }
                    }

                }
                return model;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> DeteleBook(string ID)
        {
            try
            {
                var Model = await _dbcontext.Sachs.Where(c => c.ID_Sach.Equals(ID)).FirstOrDefaultAsync();
                if (Model == null)
                {
                    return "Sách không tồn tại";
                }
                Model.TrangThai = 0;
                _dbcontext.Update(Model);
                await _dbcontext.SaveChangesAsync();
                return "Delete Done";
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }

        public async Task<List<BookModel>> GetListBook()
        {
            try
            {
                return await Task.Run(() =>
                {

                    var model = (from book in _dbcontext.Sachs
                                 where book.TrangThai == 1
                                 && book.SoLuong > 1
                                 && book.SoLuongKho > 10
                                 select new BookModel
                                 {
                                     ID_Sach = book.ID_Sach,
                                     MaNXB = book.MaNXB,
                                     TenSach = book.TenSach,
                                     HinhAnh = book.HinhAnh,
                                     SoLuong = book.SoLuong,
                                     SoTrang = book.SoTrang,
                                     TaiBan = book.TaiBan,
                                     GiaBan = book.GiaBan,
                                     SoLuongKho = book.SoLuongKho,
                                 }).ToListAsync();
                    return model;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UpdateBook(UpdateBook input)
        {
            try
            {

                if (String.IsNullOrEmpty(input.TenSach) || input.SoTrang < 20 || input.GiaBan == 0 || input.SoLuong == 0)
                {
                    return "sách không tồn tại";
                }
                var model = await _dbcontext.Sachs.Where(c => c.ID_Sach == input.ID_Sach).FirstOrDefaultAsync();

                model.TenSach = input.TenSach;
                model.SoTrang = input.SoTrang;
                model.SoLuong = input.SoLuong;
                model.TaiBan = input.TaiBan;
                model.GiaBan = input.GiaBan;
                model.MaNXB = input.MaNXB;            
                _dbcontext.Update(model);
                var rs =await _dbcontext.SachCTs.Where(x => x.MaSach.Equals(input.ID_Sach)).ToListAsync();
                foreach (var item in rs)
                {
                    _dbcontext.Remove(item);
                }                
                if (input.ListTacGia != null)
                {
                    foreach (var tg in input.ListTacGia)
                    {
                        var TacGia = await _dbcontext.TacGias.Where(c => c.ID_TacGia == tg).FirstOrDefaultAsync();
                        if (TacGia == null)
                        {
                            return "tác giả k tồn tại";
                        }
                        foreach (var tl in input.ListTheLoai)
                        {
                            var TheLoai = await _dbcontext.TheLoais.Where(c => c.ID_TheLoai == tl).FirstOrDefaultAsync();
                            if (TheLoai == null)
                            {
                                return "thể loại k tồn tại";
                            }
                            var AuthorBook = new SachCT
                            {
                                ID_SachCT = Guid.NewGuid().ToString(),
                                MaTacGia = tg,
                                MaTheLoai = tl,
                                MaSach = model.ID_Sach

                            };
                            await _dbcontext.AddAsync(AuthorBook);
                            await _dbcontext.SaveChangesAsync();
                        }
                    }

                }
               
                return "Thành công";

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
