using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Book;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
namespace WebNewBook.API.Repository.Service
{
    public class BookService : IBookSevice
    {
        private readonly dbcontext _dbcontext;
        public BookService(dbcontext dbcontext)
        {
                _dbcontext=dbcontext;
        }
        public async Task<List<BookModel>> GetListBook()
        {
            try
            {
                return await Task.Run(() =>
                {

                    var model = (from book in _dbcontext.Sachs
                                 where book.TrangThai==1
                                 && book.SoLuong>1
                                 && book.SoLuongKho>10
                                 select new BookModel
                                 {
                                     ID_Sach = book.ID_Sach,
                                     MaNXB = book.MaNXB,
                                     TenSach = book.TenSach,
                                     HinhAnh = book.HinhAnh,
                                     SoLuong=book.SoLuong,
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
    }
}
