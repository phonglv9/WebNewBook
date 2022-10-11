using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using Microsoft.EntityFrameworkCore;

namespace WebNewBook.API.Repository.Service
{
    public class Voucher : IVoucherService
    {
        private readonly dbcontext _dbcontext;

        public Voucher(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task AddVouCherAsync(Model.Voucher voucher)
        {
            throw new NotImplementedException();
        }

        public Task DeleteVouCherAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Model.Voucher>> GetVouCherAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Model.Voucher?> GetVouCherByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateVouCherAsync(Model.Voucher voucher)
        {
            throw new NotImplementedException();
        }
    }
}
