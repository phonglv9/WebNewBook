using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;


namespace WebNewBook.API.Repository.Service
{
    public class VoucherCTServices : IVoucherCTServices
    {
        private readonly dbcontext _dbcontext;
 


        public VoucherCTServices(dbcontext dbcontext, IConfiguration configuration)
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

        public async Task AddImportExcerAsync(IFormFile file ,string Phathanh)
        {
            try
            {
                var list = new List<VoucherCT>();
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet= package.Workbook.Worksheets[0];
                        var rowcount =worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowcount; row++)
                        {
                            list.Add(new VoucherCT
                            {
                             Id = worksheet.Cells[row,1].Value.ToString().Trim(),
                             NgayBatDau = null,
                            TrangThai = 0,
                            CreateDate = DateTime.Now,
                            MaVoucher = Phathanh

                            });
                        }
                    }

                }

                foreach (var lst in list)
                {
                    _dbcontext.Add(lst);
                    await _dbcontext.SaveChangesAsync();
                }


            }
            catch (Exception)
            {

                throw;
            }
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
