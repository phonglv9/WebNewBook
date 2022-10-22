using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;


namespace WebNewBook.API.Repository.Service
{
    public class VoucherCTServices : IVoucherCTServices
    {
        private readonly dbcontext _dbcontext;

        public VoucherCTServices(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        private Random random = new Random();
        public string RandomVoucher(int length)
        {
             string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(characters, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task AddAutomaticallyAsync(int quantityVoucher, int lengthVoucher , string startTextVoucher, string endTextVoucher, string maVoucher)
        {
           
            VoucherCT voucherCT = new VoucherCT();
            lengthVoucher = lengthVoucher - startTextVoucher.Length - endTextVoucher.Length;
            for (int i = 0; i < quantityVoucher; i++)
            {

                string ktRandom = RandomVoucher(lengthVoucher);
                string idVoucher = startTextVoucher + ktRandom + endTextVoucher;
                voucherCT.Id = idVoucher;
                voucherCT.NgayBatDau = null;
                voucherCT.TrangThai = 0;
                voucherCT.CreateDate = DateTime.Now;
                voucherCT.MaVoucher = maVoucher;
                _dbcontext.Add(voucherCT);
                await _dbcontext.SaveChangesAsync();
            }
         
           
        }

        public Task AddImportExcerAsync(VoucherCT voucherCT)
        {
            throw new NotImplementedException();
        }

        public async Task AddManuallyAsync(VoucherCT voucherCT)
        {
            voucherCT.NgayBatDau = null;
            voucherCT.TrangThai = 0;
            voucherCT.CreateDate = DateTime.Now;

            _dbcontext.Add(voucherCT);
            await _dbcontext.SaveChangesAsync();
        }

        public Task<VoucherCT?> GetVoucherByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VoucherCT?>> GetVoucherByMaVoucherAsync(string id)
        {
            return await _dbcontext.VoucherCTs.Where(c => c.MaVoucher == id).ToListAsync();
        }

        public async Task<IEnumerable<VoucherCT>> GetVoucherChuaphathanhAsync()
        {
            return await _dbcontext.VoucherCTs.Where(c=>c.TrangThai==0).ToListAsync();
        }

        public Task<IEnumerable<VoucherCT>> GetVoucherDaphathanhAsync()
        {
            throw new NotImplementedException();
        }

        public Task HuyVouCherAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task PhathanhVouCherAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
