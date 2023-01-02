﻿using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
	//public class GioHangService : IGioHangService
	//{
 //       private readonly dbcontext _dbContext;
 //       public GioHangService(dbcontext dbContext)
 //       {
            

 //           _dbContext = dbContext;
 //       }

 //       public async Task<int> AddGioHangAsync(string HinhAnh, int SoLuongs, string emailKH, string idsp)
 //       {
 //           var listsp = _dbContext.SanPhams.ToList();
 //           var listgh = _dbContext.GioHangs.ToList();
 //           var giohang = listgh.SingleOrDefault(c => c.Maasp == idsp);
 //           var sanpham = listsp.SingleOrDefault(c => c.ID_SanPham == idsp);
           
 //           GioHang giohangs = new GioHang();
 //           giohangs.ID_GioHang= "GH" + Guid.NewGuid().ToString();
 //           giohangs.HinhAnh = HinhAnh;
 //           giohangs.Soluong = 0;
 //           giohangs.emailKH = emailKH; 
 //           giohangs.Maasp = idsp;
 //           giohangs.Tensp = listsp.Where(o => o.ID_SanPham == idsp).Select(c => c.TenSanPham).FirstOrDefault();
 //           giohangs.DonGia= listsp.Where(o => o.ID_SanPham == idsp).Select(c => c.GiaBan).FirstOrDefault();
 //             if (listgh.Exists(c => c.Maasp == idsp&&c.emailKH==emailKH))
 //           {
                
               
 //                if (SoLuongs + giohang.Soluong > sanpham.SoLuong || giohang.Soluong > sanpham.SoLuong)
 //               {

 //                   return 2;
 //               }
               
 //               else 
 //               {
                    
 //                   giohang.Soluong+= SoLuongs;
 //                   _dbContext.Update(giohang);

 //                   await _dbContext.SaveChangesAsync();
 //                   return 3;
    
 //               }


 //           }
 //           else
             
 //           {
 //               if (SoLuongs > sanpham.SoLuong)
 //               {

 //                   return 2;
 //               }
 //               giohangs.Soluong += SoLuongs;
 //           }
 //           _dbContext.Add(giohangs);
 //           await _dbContext.SaveChangesAsync();
 //           return 3;
 //       }

 //       public async Task<List<GioHang>> GetlistGH()
 //       {
 //           var listgh= await _dbContext.GioHangs.ToListAsync();
           
 //           return listgh;
 //       }

 //       public async Task<SanPham> GetSanPham( string ID)
 //       { 
            
 //           var b =  _dbContext.SanPhams.SingleOrDefault(a=>a.ID_SanPham==ID);

 //           return  b ;
 //       }

 //       public async Task<int> Updatenumber(string id, int soluongmoi, string namekh,string update)
 //       {
 //           var listsp = _dbContext.SanPhams.ToList();
 //           var listgh = _dbContext.GioHangs.ToList();
 //           var giohang = listgh.SingleOrDefault(c => c.Maasp == id && c.emailKH == namekh);
 //           var Sanpham = listsp.SingleOrDefault(c => c.ID_SanPham == id);
 //           if (soluongmoi != 0) {
               
 //               if (soluongmoi != 0)
 //               {
 //                   if (soluongmoi <= Sanpham.SoLuong)
 //                   {
 //                       giohang.Soluong = soluongmoi;
 //                       giohang.ThanhTien = giohang.Soluong * giohang.DonGia;
 //                       _dbContext.Update(giohang);
 //                       await _dbContext.SaveChangesAsync();
 //                       return 3;
 //                   }
 //                   else
 //                   {
 //                       return 2;
 //                   }
 //               }
 //           }
 //           else
 //           {
 //               if (update == "1") {
 //                   giohang.Soluong = giohang.Soluong+1;
 //                   giohang.ThanhTien = giohang.Soluong * giohang.DonGia;
 //                   if (giohang.Soluong + soluongmoi > Sanpham.SoLuong)
 //                   {
                      
 //                       return 2;
 //                   }
 //               }
 //               else if(update == "2")
 //               {
 //                   giohang.Soluong = giohang.Soluong - 1;
 //                   giohang.ThanhTien = giohang.Soluong * giohang.DonGia;
 //                   if (giohang.Soluong <= 0)
 //                   {

 //                       var listghang = _dbContext.GioHangs.ToList();
 //                       var xgiohang = listghang.SingleOrDefault(c => c.Maasp == id && c.emailKH == namekh);
 //                       _dbContext.Remove(xgiohang);
 //                       await _dbContext.SaveChangesAsync();

 //                   }
 //               }
 //               _dbContext.Update(giohang);
 //               await _dbContext.SaveChangesAsync();
 //               return 3;

 //           }
           
         
            
 //           return 0;

 //       }

  

 //       public async Task<string> XoakhoiGioHang(string id,string namekh)
 //       {
 //           var listgh = _dbContext.GioHangs.ToList();
 //           var giohang = listgh.SingleOrDefault(c => c.Maasp ==id&&c.emailKH==namekh );
 //           _dbContext.Remove(giohang);
 //           await _dbContext.SaveChangesAsync();


 //           return "xoa thành công";
 //       }
 //       //phóng code
 //       public async Task XoaGioHangKH(string email)
 //       {
 //           var listgh = _dbContext.GioHangs.Where(c=>c.emailKH == email).ToList();
   
 //           _dbContext.RemoveRange(listgh);
 //           await _dbContext.SaveChangesAsync();
 //       }

 //       public async Task<int> getSP(string id)
 //       {
            
 //           var listsp = await _dbContext.SanPhams.ToListAsync();
 //           var soluong = listsp.Where(c => c.ID_SanPham == id).Select(c => c.SoLuong).FirstOrDefault();

 //           return soluong;
 //       }

 //       public async Task<int> ChecksoluongCart()
 //       {
 //           var trangthai = 0;
 //           var listgh=await _dbContext.GioHangs.ToListAsync();
 //           var listsp = await _dbContext.SanPhams.ToListAsync();
 //           foreach(var cm in listgh)
 //           {
 //               var soluongsp = listsp.Where(b => b.ID_SanPham == cm.Maasp).Select(c => c.SoLuong).FirstOrDefault();
 //               if (cm.Soluong> soluongsp)
 //               {
 //                   cm.Soluong= soluongsp;
 //                   _dbContext.Update(cm);
                  
 //                   trangthai =1;

 //               }

 //           }
 //           _dbContext.SaveChangesAsync();
 //           return trangthai;
 //       }

       
 //   }
}
