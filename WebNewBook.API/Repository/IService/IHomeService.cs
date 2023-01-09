using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IHomeService
    {
        Task<IEnumerable<HomeViewModel>> GetHomVM();
        Task<IEnumerable<ProductVM>> GetProductHome();

        Task<List<TheLoai>> GetTheLoais();
        Task<List<DanhMucSach>> GetDanhMucNavBar();
        Task<List<TacGia>> GetTacGias();
        Task<SanPhamChiTiet> GetProductDetail(string id);
        Task<List<SanPhamChiTiet>> GetTheLoaisCT(string id);
        Task<List<ProductOderTop10VM>> GetTopProduct10Oder();



    }
}
