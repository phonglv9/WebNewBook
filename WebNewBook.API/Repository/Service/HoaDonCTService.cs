using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class HoaDonCTService : IHoaDonCTService
    {
        private readonly dbcontext _dbcontext;

        public HoaDonCTService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

   
        public async Task AddOrderDetail(string mhd, string masp)
        {

            var giabansp = _dbcontext.SanPhams.FirstOrDefault(c => c.ID_SanPham == masp).GiaBan;
            HoaDonCT hoaDonCT = new HoaDonCT
            {
                ID_HDCT = "hdct" + Guid.NewGuid().ToString(),
                MaSanPham = masp,
                MaHoaDon = mhd,
                SoLuong = 1,
                GiaBan = giabansp
            };
            var product = _dbcontext.SanPhams.FirstOrDefault(c => c.ID_SanPham == masp);

            var checktrungSp = _dbcontext.HoaDonCTs.Any(c => c.MaHoaDon == mhd && c.MaSanPham == masp);
            Console.WriteLine(checktrungSp);
            if (checktrungSp)
            {
                var mahdct = _dbcontext.HoaDonCTs.Where(c => c.MaSanPham == masp && c.MaHoaDon == mhd).Select(c => c.ID_HDCT).FirstOrDefault();
                var model = _dbcontext.HoaDonCTs.FirstOrDefault(c => c.ID_HDCT == mahdct);
                model.SoLuong += 1;
                product.SoLuong -= 1;
                _dbcontext.HoaDonCTs.Update(model);
                _dbcontext.SanPhams.Update(product);
             
            }
            else
            {
                product.SoLuong -= 1;
                _dbcontext.HoaDonCTs.Add(hoaDonCT);
                _dbcontext.SanPhams.Update(product);

            }
            await _dbcontext.SaveChangesAsync();
          


            double tong = 0;
            var hoaDON = _dbcontext.HoaDonCTs.Where(c => c.MaHoaDon == mhd).ToList();
            foreach (var x in hoaDON)
            {
                var thanhtien = x.GiaBan * x.SoLuong;
                tong += thanhtien;
            }
            var updatehoadon = _dbcontext.HoaDons.FirstOrDefault(c => c.ID_HoaDon == mhd);

            updatehoadon.TongTien = tong;
            if (updatehoadon.MaGiamGia != null)
            {
               var voucherct= _dbcontext.VoucherCTs.FirstOrDefault(c => c.Id == updatehoadon.MaGiamGia);
                if (voucherct!=null)
                {
                    var voucher = _dbcontext.Vouchers.FirstOrDefault(v => v.Id == voucherct.MaVoucher);
                    if (tong>= voucher.MenhGiaDieuKien)
                    {
                        updatehoadon.TongTien -= voucher.MenhGia;
                    }
                    else
                    {
                        voucherct.TrangThai = 1;
                        _dbcontext.VoucherCTs.Update(voucherct);
                        updatehoadon.MaGiamGia = null;

                      
                    }
                }
               
            }
            _dbcontext.HoaDons.Update(updatehoadon);
            _dbcontext.SaveChanges();


        }

        public async Task DeletaOrderDetail(string mhdct)
        {
            var modal = _dbcontext.HoaDonCTs.FirstOrDefault(c => c.ID_HDCT == mhdct);
            var product = _dbcontext.SanPhams.FirstOrDefault(c => c.ID_SanPham == modal.MaSanPham);
            product.SoLuong += modal.SoLuong;
            _dbcontext.SanPhams.Update(product);
            _dbcontext.HoaDonCTs.Remove(modal);
            await _dbcontext.SaveChangesAsync();

            double tong = 0;
            var hoaDON = _dbcontext.HoaDonCTs.Where(c => c.MaHoaDon == modal.MaHoaDon).ToList();
            foreach (var x in hoaDON)
            {
                var thanhtien = x.GiaBan * x.SoLuong;
                tong += thanhtien;
            }
            var updatehoadon = _dbcontext.HoaDons.FirstOrDefault(c => c.ID_HoaDon == modal.MaHoaDon);
            updatehoadon.TongTien = tong;
            if (updatehoadon.MaGiamGia != null)
            {
                var voucherct = _dbcontext.VoucherCTs.FirstOrDefault(c => c.Id == updatehoadon.MaGiamGia);
                if (voucherct != null)
                {
                    var voucher = _dbcontext.Vouchers.FirstOrDefault(v => v.Id == voucherct.MaVoucher);
                    if (tong >= voucher.MenhGiaDieuKien)
                    {
                        updatehoadon.TongTien -= voucher.MenhGia;
                    }
                    else
                    {
                        voucherct.TrangThai = 1;
                        _dbcontext.VoucherCTs.Update(voucherct);
                        updatehoadon.MaGiamGia = null;
                     
                    }
                }

            }
            _dbcontext.HoaDons.Update(updatehoadon);
            _dbcontext.SaveChanges();
        }

        public async Task UpdateOrderDetailQuantity(string mhdct, int quantity)
        {
            var modal = _dbcontext.HoaDonCTs.FirstOrDefault(c => c.ID_HDCT == mhdct );
            var soluongtruoc = modal.SoLuong;
            modal.SoLuong = quantity;
            var product = _dbcontext.SanPhams.FirstOrDefault(c => c.ID_SanPham == modal.MaSanPham);
            if (soluongtruoc<quantity)
            {
                var soluongcanlay = quantity - soluongtruoc;
                product.SoLuong -= soluongcanlay;
                _dbcontext.SanPhams.Update(product);
            }
            if (soluongtruoc>quantity)
            {
                var soluongcanlay =  soluongtruoc-quantity;
                product.SoLuong += soluongcanlay;
                _dbcontext.SanPhams.Update(product);
            }
           
            _dbcontext.HoaDonCTs.Update(modal);
             await _dbcontext.SaveChangesAsync();

            double tong = 0;
            var hoaDON = _dbcontext.HoaDonCTs.Where(c => c.MaHoaDon == modal.MaHoaDon).ToList();
            foreach (var x in hoaDON)
            {
                var thanhtien = x.GiaBan * x.SoLuong;
                tong += thanhtien;
            }
            var updatehoadon = _dbcontext.HoaDons.FirstOrDefault(c => c.ID_HoaDon == modal.MaHoaDon);
            updatehoadon.TongTien = tong;
            if (updatehoadon.MaGiamGia != null)
            {
                var voucherct = _dbcontext.VoucherCTs.FirstOrDefault(c => c.Id == updatehoadon.MaGiamGia);
                if (voucherct != null)
                {
                    var voucher = _dbcontext.Vouchers.FirstOrDefault(v => v.Id == voucherct.MaVoucher);
                    if (tong >= voucher.MenhGiaDieuKien)
                    {
                        updatehoadon.TongTien -= voucher.MenhGia;
                    }
                    else
                    {
                        voucherct.TrangThai = 1;
                        _dbcontext.VoucherCTs.Update(voucherct);
                        updatehoadon.MaGiamGia = null;
                 
                    }
                }

            }
            _dbcontext.HoaDons.Update(updatehoadon);
            _dbcontext.SaveChanges();
        }
    }
}
