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
        private IVoucherService _voucherService;
        public VoucherCTServices(dbcontext dbcontext, IConfiguration configuration, IVoucherService voucherService)
        {
            _dbcontext = dbcontext;
            _voucherService = voucherService;
        }

        private Random random = new Random();
        public string RandomVoucher(int length)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(characters, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task AddAutomaticallyAsync(int quantityVoucher, int lengthVoucher, string startTextVoucher, string endTextVoucher, string maVoucher)
        {

            try
            {
                DateTime ngayKetThuc = _dbcontext.Vouchers.FirstOrDefault(c => c.Id == maVoucher).EndDate;
                VoucherCT voucherCT = new VoucherCT();
                lengthVoucher = lengthVoucher - startTextVoucher.Length - endTextVoucher.Length;
                for (int i = 0; i < quantityVoucher; i++)
                {
                    dynamic checkTrung;
                    do
                    {
                        string ktRandom = RandomVoucher(lengthVoucher);
                        string idVoucher = startTextVoucher + ktRandom + endTextVoucher;
                        checkTrung = _dbcontext.VoucherCTs.FirstOrDefault(c => c.Id == idVoucher);
                        voucherCT.Id = idVoucher;
                    } while (checkTrung != null);


                    voucherCT.NgayBatDau = null;
                    voucherCT.TrangThai = 0;
                    voucherCT.NgayHetHan = ngayKetThuc;
                    voucherCT.CreateDate = DateTime.Now;
                    voucherCT.MaVoucher = maVoucher;
                    _dbcontext.VoucherCTs.Add(voucherCT);

                    _dbcontext.Vouchers.Update(SoluongVoucherCT(maVoucher));
                    await _dbcontext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task AddImportExcerAsync(IFormFile file, string Phathanh)
        {
            try
            {
                if (file != null)
                {

                    DateTime ngayKetThuc = _dbcontext.Vouchers.FirstOrDefault(c => c.Id == Phathanh).EndDate;
                    var list = new List<VoucherCT>();
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                            var rowcount = worksheet.Dimension.Rows;
                            for (int row = 2; row <= rowcount; row++)
                            {
                                list.Add(new VoucherCT
                                {
                                    Id = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                    NgayBatDau = null,
                                    TrangThai = 0,
                                    NgayHetHan = ngayKetThuc,
                                    CreateDate = DateTime.Now,
                                    MaVoucher = Phathanh

                                }); ;
                            }
                        }

                    }
                    if (list != null)
                    {

                        foreach (var lst in list)
                        {
                            _dbcontext.Add(lst);
                            await _dbcontext.SaveChangesAsync();
                        }
                        var soluongVoucherCT = _dbcontext.VoucherCTs.Count(c => c.MaVoucher == Phathanh);
                        var model = _dbcontext.Vouchers.FirstOrDefault(c => c.Id == Phathanh);
                        model.SoLuong = soluongVoucherCT;
                        model.TrangThai = 1;
                        _dbcontext.Vouchers.Update(model);
                        await _dbcontext.SaveChangesAsync();

                    }
                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public Voucher SoluongVoucherCT(string mavoucher)
        {
            var modelVoucher = _dbcontext.Vouchers.FirstOrDefault(c => mavoucher == c.Id);
            if (modelVoucher != null)
                modelVoucher.TrangThai = 1;
                modelVoucher.SoLuong += 1;
            return modelVoucher;
        }
        public async Task AddManuallyAsync(VoucherCT voucherCT)
        {
            try
            {
                if (voucherCT != null)
                {
                    voucherCT.NgayBatDau = null;
                    voucherCT.TrangThai = 0;
                    voucherCT.CreateDate = DateTime.Now;
               
                  
                    _dbcontext.VoucherCTs.Add(voucherCT);
                    _dbcontext.Vouchers.Update(SoluongVoucherCT(voucherCT.MaVoucher));

                    await _dbcontext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<VoucherCT?> GetVoucherByIdAsync(string id)
        {
            try
            {
                return await _dbcontext.VoucherCTs.FirstOrDefaultAsync(c => c.Id == id) ?? null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<VoucherCT?>> GetVoucherByMaVoucherAsync(string id)
        {
            try
            {
                return await _dbcontext.VoucherCTs.Where(c => c.MaVoucher == id).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<VoucherCT>> GetVoucherChuaphathanhAsync()
        {
            try
            {
                return await _dbcontext.VoucherCTs.Where(c => c.TrangThai == 0).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<VoucherCT>> GetVoucherDaphathanhAsync()
        {
            try
            {
                return await _dbcontext.VoucherCTs.Where(c => c.TrangThai == 1).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task HuyVouCherAsync(List<string> id)
        {
            try
            {

                if (id != null)
                {
                    foreach (var x in id)
                    {
                        var modal = await _dbcontext.VoucherCTs.FindAsync(x);
                        modal.TrangThai = 3;
                        _dbcontext.VoucherCTs.Update(modal);
                        await _dbcontext.SaveChangesAsync();
                    }

                }


            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public async Task PhathanhVouCherAsync(List<VoucherCT> lstvoucherCTs)
        {
            try
            {
                int i = 0;
                var lstCustomer = _dbcontext.KhachHangs.Where(c=>c.ID_KhachHang!="KHNOLOGIN").ToList();

                if (lstvoucherCTs != null)
                {
                    foreach (var x in lstvoucherCTs)
                    {
                        var model = await _dbcontext.VoucherCTs.FindAsync(x.Id);


                        if (model != null)
                        {
                            var voucher = _dbcontext.Vouchers.Where(c => c.Id == model.MaVoucher).FirstOrDefault();
                            if (voucher != null)
                            {
                                if (voucher.HinhThuc == 2 && lstCustomer[i] != null)
                                {
                                    if (x.MaKhachHang!=null)
                                    {
                                        model.MaKhachHang = x.MaKhachHang;
                                     
                                    }
                                    else
                                    {
                                        if (lstvoucherCTs.Count >= lstCustomer.Count)
                                        {
                                            model.MaKhachHang = lstCustomer[i].ID_KhachHang;
                                            i++;
                                        }
                                    }
                                    model.TrangThai = 1;
                                    model.HinhThuc = voucher.HinhThuc;
                                    model.Diemdoi = voucher.DiemDoi;
                                    model.NgayBatDau = x.NgayBatDau;
                                    _dbcontext.VoucherCTs.Update(model);
                                    await _dbcontext.SaveChangesAsync();
                                    if (i == lstCustomer.Count)
                                    {
                         
                                        break;
                                    }
                                }
                                if (voucher.HinhThuc == 1)
                                {

                                    model.TrangThai = 1;
                                    model.HinhThuc = voucher.HinhThuc;
                                    model.Diemdoi = voucher.DiemDoi;
                                    model.NgayBatDau = x.NgayBatDau;
                                    _dbcontext.VoucherCTs.Update(model);
                                    await _dbcontext.SaveChangesAsync();
                                }
                            }

                        }



                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IEnumerable<VoucherCT>> GetVoucherOfCustomer(string maCustomer)
        {
            try
            {
                var datenow = DateTime.Now;
                var model = await _dbcontext.VoucherCTs.Where(c => c.TrangThai == 1 && c.MaKhachHang == maCustomer && datenow <= c.NgayHetHan).ToListAsync();
                return model;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task UpdateVoucherByPayment(string idVoucherCT)
        {
            try
            {
                if (!string.IsNullOrEmpty(idVoucherCT))
                {
                    var voucherCT = _dbcontext.VoucherCTs.Where(c => c.Id == idVoucherCT).FirstOrDefault();
                    if (voucherCT != null)
                    {
                        voucherCT.NgaySuDung = DateTime.Now;
                        voucherCT.TrangThai = 2;
                        _dbcontext.Update(voucherCT);
                        await _dbcontext.SaveChangesAsync();
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<VoucherCT>> ListVoucherCTByPayment(string id)
        {
            var dateNow = DateTime.Now;
            var listVCCT = await _dbcontext.VoucherCTs.Where(C => C.MaKhachHang == id && dateNow >= C.NgayBatDau && dateNow <= C.NgayHetHan).ToListAsync();
            return listVCCT;
        }

        public async Task<IEnumerable<VoucherCT>> GetAddVoucherCT()
        {
            return await _dbcontext.VoucherCTs.ToListAsync();
        }

     

        public async Task DoiDiemVoucher(string id, string maKh)
        {
            try
            {
                if (id != null && maKh != null)
                {
                    
                    var lstVoucherCT = _dbcontext.VoucherCTs.Where(c => c.TrangThai == 0 && c.MaVoucher == id  && c.MaKhachHang==null).ToList();
                    Voucher Voucher = _dbcontext.Vouchers.Where(c => c.Id == id).FirstOrDefault();
                    var Customer = _dbcontext.KhachHangs.Where(c => c.ID_KhachHang == maKh).FirstOrDefault();
                    if (  Voucher != null && Customer!=null)
                    {
                        VoucherCT voucherCT = lstVoucherCT[0];
                        if (lstVoucherCT.Count==1)
                        {
                            Voucher.TrangThai = 3;
                        }
                        voucherCT.MaKhachHang = maKh;
                        voucherCT.NgayBatDau = DateTime.Now;
                        voucherCT.TrangThai = 1;
                        voucherCT.HinhThuc = Voucher.HinhThuc;
                        voucherCT.Diemdoi = Voucher.DiemDoi;

                        Customer.DiemTichLuy = (int)(Customer.DiemTichLuy - Voucher.DiemDoi);




                        _dbcontext.VoucherCTs.Update(voucherCT);
                        _dbcontext.KhachHangs.Update(Customer);
                        _dbcontext.Vouchers.Update(Voucher);
                    }
                    await _dbcontext.SaveChangesAsync();

                }
            }
            catch (Exception e)
            {

                throw e;
            }
          
        }

        public async Task<HoaDon?> GetOderByIdVoucherCT(string id)
        {
            return _dbcontext.HoaDons.FirstOrDefault(c => c.MaGiamGia == id);
        }
    }
}
