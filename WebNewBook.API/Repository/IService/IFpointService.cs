using WebNewBook.Model;
namespace WebNewBook.API.Repository.IService
{
    public interface IFpointService
    {
        Task<IEnumerable<Fpoint>> GetListFpoint();
        Task AddFoint(string id , string diemtichluy, string makhachhang);
    }
}
