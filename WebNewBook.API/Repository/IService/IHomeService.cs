using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.ModelsAPI;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.IService
{
    public interface IHomeService
    {
        Task<List<HomeVM>> GetHomVM();
        Task<List<HomeVM>> GetProductHome();
        Task<List<TheLoai>> GetTheLoais();
        Task<List<DanhMucSach>> GetDanhMucs();


    }
}
