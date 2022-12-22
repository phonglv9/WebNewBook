using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
namespace WebNewBook.API.Repository.Service
{
    public class FpointService : IFpointService
    {
        public readonly dbcontext _dbcontext;
        public FpointService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task AddFoint(string id, string diemtichluy, string makhachhang)
        {
            try
            {
                if (!(string.IsNullOrEmpty(id) && string.IsNullOrEmpty(diemtichluy) && string.IsNullOrEmpty(makhachhang)))
                {
                    Fpoint fpoint = new Fpoint();
                    fpoint.Id= id;
                    fpoint.Diemtichluy = Convert.ToDouble(diemtichluy);
                    fpoint.Hanhdong = 1;
                    //hanh động 1 : tích điểm fpoint
                    //hành động 2 : nạp điểm fpoint
                    //hành động 3 : đổi điểm fpoint
                    if (fpoint.Hanhdong==1)
                    {
                        fpoint.NameHanhDong = "Tích điểm Point";
                    }
                    if (fpoint.Hanhdong==2)
                    {
                        fpoint.NameHanhDong = "Nạp điểm Point";
                    }
                    fpoint.MaKhachHang = makhachhang;
                    fpoint.CreatDate = DateTime.Now;
                    fpoint.TrangThai = 1;
                   var customer= _dbcontext.KhachHangs.FirstOrDefault(c => c.ID_KhachHang == makhachhang);
                    customer.DiemTichLuy += Convert.ToInt32(fpoint.Diemtichluy);
                    _dbcontext.Add(fpoint);
                
                    _dbcontext.Update(customer);
                 

                }
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Fpoint>> GetListFpoint()
        {
            try
            {
                return await _dbcontext.Fpoints.ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
