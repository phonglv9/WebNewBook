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

        public async Task<int> AddGioHangAsync(string HinhAnh, int SoLuongs, string emailKH, string idsp)
        {
            var listsp = _dbContext.SanPhams.ToList();
            var listgh = _dbContext.GioHangs.ToList();
            var giohang = listgh.SingleOrDefault(c => c.Maasp == idsp);
            var sanpham = listsp.SingleOrDefault(c => c.ID_SanPham == idsp);
           
            GioHang giohangs = new GioHang();
            giohangs.ID_GioHang= "GH" + Guid.NewGuid().ToString();
            giohangs.HinhAnh = HinhAnh;
            giohangs.Soluong = 0;
            giohangs.emailKH = emailKH;
            giohangs.Maasp = idsp;
            giohangs.Tensp = listsp.Where(o => o.ID_SanPham == idsp).Select(c => c.TenSanPham).FirstOrDefault();
            giohangs.DonGia= listsp.Where(o => o.ID_SanPham == idsp).Select(c => c.GiaBan).FirstOrDefault();
              if (listgh.Exists(c => c.Maasp == idsp&&c.emailKH==emailKH))
            {
                
               
                 if (SoLuongs + giohang.Soluong > sanpham.SoLuong || giohang.Soluong > sanpham.SoLuong)
                {

                    return 2;
                }
               
                else 
                {
                    
                    giohang.Soluong+= SoLuongs;
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
                giohangs.Soluong += SoLuongs;
            }
            _dbContext.Add(giohangs);
            await _dbContext.SaveChangesAsync();
            return 3;
        }

        public async Task<List<GioHang>> GetlistGH()
        {
            var listgh= await _dbContext.GioHangs.ToListAsync();
           
            return listgh;
        }

        public async Task<SanPham> GetSanPham( string ID)
        { 
            
            var b =  _dbContext.SanPhams.SingleOrDefault(a=>a.ID_SanPham==ID);

            return  b ;
        }

        public async Task<int> Updatenumber(string id, int soluongmoi, string namekh,string update)
        {
            var listsp = _dbContext.SanPhams.ToList();
            var listgh = _dbContext.GioHangs.ToList();
            var giohang = listgh.SingleOrDefault(c => c.Maasp == id && c.emailKH == namekh);
            var Sanpham = listsp.SingleOrDefault(c => c.ID_SanPham == id);
            if (soluongmoi != 0) {
               
                if (soluongmoi != 0)
                {
                    if (soluongmoi <= Sanpham.SoLuong)
                    {
                        giohang.Soluong = soluongmoi;
                        giohang.ThanhTien = giohang.Soluong * giohang.DonGia;
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
                if (update == "1") {
                    giohang.Soluong = giohang.Soluong+1;
                    giohang.ThanhTien = giohang.Soluong * giohang.DonGia;
                    if (giohang.Soluong + soluongmoi > Sanpham.SoLuong)
                    {
                      
                        return 2;
                    }
                }
                else if(update == "2")
                {
                    giohang.Soluong = giohang.Soluong - 1;
                    giohang.ThanhTien = giohang.Soluong * giohang.DonGia;
                    if (giohang.Soluong <= 0)
                    {

                        giohang.Soluong = 1;
                        giohang.ThanhTien = giohang.Soluong * giohang.DonGia;

                    }
                }
                _dbContext.Update(giohang);
                await _dbContext.SaveChangesAsync();
                return 3;

            }
           
         
            
            return 0;

        }

       
        //Truy vấn lại giỏ hàng:
        public async Task<List<HomeVM>> VM()
		{
            var sanPham = await _dbContext.SanPhams.ToListAsync();
            var sanPhamCT = await _dbContext.SanPhamCTs.ToListAsync();
            var sachCT = await _dbContext.SachCTs.ToListAsync();
            var sach = await _dbContext.Sachs.ToListAsync();
            var theLoai = await _dbContext.TheLoais.ToListAsync();
            var danhMuc = await _dbContext.DanhMucSachs.ToListAsync();
            var tacgia = await _dbContext.TacGias.ToListAsync();
            List<HomeVM> homeVMs = new List<HomeVM>();
            homeVMs = (from b in sanPhamCT
                       join c in sach on b.MaSach equals c.ID_Sach
                       join d in sachCT on c.ID_Sach equals d.MaSach
                       join e in theLoai on d.MaTheLoai equals e.ID_TheLoai
                       join f in danhMuc on e.MaDanhMuc equals f.ID_DanhMuc
                       join g in tacgia on d.MaTacGia equals g.ID_TacGia
                       select new HomeVM()
                       {

                           SanPhamCT = b,
                           sach = c,
                           sachCT = d,
                           theLoai = e,
                           danhMucSach = f,
                           tacGia = g,
                       }).ToList();
            return homeVMs;
        }

        public async Task<string> XoakhoiGioHang(string id,string namekh)
        {
            var listgh = _dbContext.GioHangs.ToList();
            var giohang = listgh.SingleOrDefault(c => c.Maasp ==id&&c.emailKH==namekh );
            _dbContext.Remove(giohang);
            await _dbContext.SaveChangesAsync();


            return "xoa thành công";
        }
        //phóng code
        public async Task XoaGioHangKH(string email)
        {
            var listgh = _dbContext.GioHangs.Where(c=>c.emailKH == email).ToList();
   
            _dbContext.RemoveRange(listgh);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<SanPham>> getSP()
        {
            var listsp = await _dbContext.SanPhams.ToListAsync();

            return listsp;
        }
    }
}
