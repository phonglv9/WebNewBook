using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class GioHangService : IGioHangService
    {
        private readonly dbcontext _dbContext;
        public GioHangService(dbcontext dbContext)
        {


            _dbContext = dbContext;
        }

        public async Task<int> AddGioHangAsync( int SoLuongs, string emailKH, string idsp)
        {
            var listsp = _dbContext.SanPhams.ToList();
            var listgh = _dbContext.GioHangs.ToList();
           
            var giohang = listgh.SingleOrDefault(c => c.MaSanPham == idsp);
            var sanpham = listsp.SingleOrDefault(c => c.ID_SanPham == idsp);
            var listkh = _dbContext.KhachHangs.ToList();
            var idKhachHang= listkh.Where(o => o.Email == emailKH).Select(c => c.ID_KhachHang).FirstOrDefault();

            GioHang giohangs = new GioHang();
            giohangs.ID_GioHang = "GH" + Guid.NewGuid().ToString();
            
            giohangs.SoLuong = 0;
            giohangs.MaKhachHang = idKhachHang;
            giohangs.MaSanPham = idsp;
            //giohangs.Tensp = listsp.Where(o => o.ID_SanPham == idsp).Select(c => c.TenSanPham).FirstOrDefault();
            //giohangs.DonGia = listsp.Where(o => o.ID_SanPham == idsp).Select(c => c.GiaBan).FirstOrDefault();
            if (listgh.Exists(c => c.MaSanPham == idsp && c.MaKhachHang == idKhachHang))
            {


                if (SoLuongs + giohang.SoLuong > sanpham.SoLuong || giohang.SoLuong > sanpham.SoLuong)
                {

                    return 2;
                }

                else
                {

                    giohang.SoLuong += SoLuongs;
                    _dbContext.Update(giohang);

                    await _dbContext.SaveChangesAsync();
                    return 3;

                }


            }
            else

            {
                if (SoLuongs > sanpham.SoLuong)
                {

                    return 2;
                }
                giohangs.SoLuong += SoLuongs;
            }
            _dbContext.Add(giohangs);
            await _dbContext.SaveChangesAsync();
            return 3;
        }

        public async Task<List<ModelCart>> GetlistGH()
        {
            var GioHang = _dbContext.GioHangs;
            var KhachHang = _dbContext.KhachHangs;
            var HoaDon = _dbContext.HoaDons;
            var HoaDonCT = _dbContext.HoaDonCTs;
            var SanPham = _dbContext.SanPhams;

            var products = (from a in GioHang
                            join b in KhachHang on a.MaKhachHang equals b.ID_KhachHang

                            join e in SanPham on a.MaSanPham equals e.ID_SanPham
                          
                            select new ModelCart()
                            {
                                EmailKH = b.Email,
                                MaSanPham = a.MaSanPham,
                                SoLuong = a.SoLuong,
                                GiaBan = e.GiaBan,
                                HinhAnh = e.HinhAnh,
                                TenSanPham = e.TenSanPham,
                              


                            }).ToList();
            return products;
        }

        public async Task<SanPham> GetSanPham(string ID)
        {

            var b =  _dbContext.SanPhams.SingleOrDefault(a => a.ID_SanPham == ID);

            return  b;
        }

        public async Task<int> Updatenumber(string id, int soluongmoi, string namekh, string update)
        {
            var listsp = _dbContext.SanPhams.ToList();
            var listgh = _dbContext.GioHangs.ToList();
            var listkh = _dbContext.KhachHangs.ToList();
            var idKhachHang = listkh.Where(o => o.Email == namekh).Select(c => c.ID_KhachHang).FirstOrDefault();

            var giohang = listgh.SingleOrDefault(c => c.MaSanPham == id && c.MaKhachHang == idKhachHang);
            var Sanpham = listsp.SingleOrDefault(c => c.ID_SanPham == id);
            if (soluongmoi != 0)
            {

                if (soluongmoi != 0)
                {
                    if (soluongmoi <= Sanpham.SoLuong)
                    {
                        giohang.SoLuong = soluongmoi;
                        //giohang.ThanhTien = giohang.SoLuong * Sanpham.GiaBan;
                        _dbContext.Update(giohang);
                        await _dbContext.SaveChangesAsync();
                        return 3;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            else
            {
                if (update == "1")
                {
                    giohang.SoLuong = giohang.SoLuong + 1;
                    //giohang.ThanhTien = giohang.SoLuong * Sanpham.GiaBan;
                    if (giohang.SoLuong + soluongmoi > Sanpham.SoLuong)
                    {

                        return 2;
                    }
                }
                else if (update == "2")
                {
                    giohang.SoLuong = giohang.SoLuong - 1;
                    //giohang.ThanhTien = giohang.Soluong * giohang.DonGia;
                    if (giohang.SoLuong <= 0)
                    {

                        var listghang = _dbContext.GioHangs.ToList();
                        var xgiohang = listghang.SingleOrDefault(c => c.MaSanPham == id && c.MaKhachHang == idKhachHang);
                        _dbContext.Remove(xgiohang);
                        await _dbContext.SaveChangesAsync();

                    }
                }
                _dbContext.Update(giohang);
                await _dbContext.SaveChangesAsync();
                return 3;

            }



            return 0;

        }



        public async Task<string> XoakhoiGioHang(string id, string namekh)
        {
            var listkh = _dbContext.KhachHangs.ToList();
            var idKhachHang = listkh.Where(o => o.Email == namekh).Select(c => c.ID_KhachHang).FirstOrDefault();

            var listgh = _dbContext.GioHangs.ToList();
            var giohang = listgh.SingleOrDefault(c => c.MaSanPham == id && c.MaKhachHang == idKhachHang);
            _dbContext.Remove(giohang);
            await _dbContext.SaveChangesAsync();


            return "xoa thành công";
        }
        //phóng code
        public async Task XoaGioHangKH(string email)
        {
            var listkh = _dbContext.KhachHangs.ToList();
            var idKhachHang = listkh.Where(o => o.Email == email).Select(c => c.ID_KhachHang).FirstOrDefault();

            var listgh = _dbContext.GioHangs.Where(c => c.MaKhachHang == idKhachHang).ToList();

            _dbContext.RemoveRange(listgh);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> getSP(string id)
        {

            var listsp = await _dbContext.SanPhams.ToListAsync();
            var soluong = listsp.Where(c => c.ID_SanPham == id).Select(c => c.SoLuong).FirstOrDefault();

            return soluong;
        }

        public async Task<int> ChecksoluongCart()
        {
            var trangthai = 0;
            var listgh = await _dbContext.GioHangs.ToListAsync();
            var listsp = await _dbContext.SanPhams.ToListAsync();
            foreach (var cm in listgh)
            {
                var soluongsp = listsp.Where(b => b.ID_SanPham == cm.MaSanPham).Select(c => c.SoLuong).FirstOrDefault();
                if (cm.SoLuong > soluongsp)
                {
                    cm.SoLuong = soluongsp;
                    _dbContext.Update(cm);

                    trangthai = 1;

                }

            }
            _dbContext.SaveChangesAsync();
            return trangthai;
        }


    }
}
