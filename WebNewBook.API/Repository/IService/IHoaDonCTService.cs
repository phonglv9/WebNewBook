namespace WebNewBook.API.Repository.IService
{
    public interface IHoaDonCTService
    {
        Task AddOrderDetail(string mhd, string masp);
        Task UpdateOrderDetailQuantity(string mhdct, int quantity);
        Task DeletaOrderDetail(string mhdct);

    }
}
