using Microsoft.EntityFrameworkCore;
using MimeKit;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;

namespace WebNewBook.API.Repository.Service
{
    public class HoaDonService : IHoaDonService
    {
        private readonly dbcontext dbcontext;

        public HoaDonService(dbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task AddHoaDon(HoaDon hoaDon)
        {


            dbcontext.HoaDons.Add(hoaDon);
            dbcontext.SaveChanges();

        }
        public async Task AddHoaDonCT(List<HoaDonCT> hoaDonCTs)
        {
            if (hoaDonCTs != null)
            {


                dbcontext.HoaDonCTs.AddRange(hoaDonCTs);
                dbcontext.SaveChanges();
            }
            else
            {
                throw null;
            }



        }
        public async Task UpdateSLSanPham(List<HoaDonCT> hoaDonCTs)
        {
            if (hoaDonCTs != null)
            {

                foreach (var item in hoaDonCTs)
                {
                    var sanPham = await dbcontext.SanPhams.Where(c => c.ID_SanPham == item.MaSanPham).FirstOrDefaultAsync();
                    if (sanPham != null)
                        sanPham.SoLuong = sanPham.SoLuong - item.SoLuong;
                    dbcontext.SanPhams.UpdateRange(sanPham);
                    await dbcontext.SaveChangesAsync();

                }

            }
            else
            {
                throw null;
            }



        }
        public async Task UpdateSLSanPhamVNPay(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                var hoaDonCTs = dbcontext.HoaDonCTs.Where(c => c.MaHoaDon == id).ToList();
                foreach (var item in hoaDonCTs)
                {
                    var sanPham = await dbcontext.SanPhams.Where(c => c.ID_SanPham == item.MaSanPham).FirstOrDefaultAsync();
                    if (sanPham != null)
                        sanPham.SoLuong = sanPham.SoLuong - item.SoLuong;
                    dbcontext.SanPhams.UpdateRange(sanPham);
                    await dbcontext.SaveChangesAsync();

                }

            }
            else
            {
                throw null;
            }



        }
        public async Task UpdateTrangThai(string id)
        {
            var hoaDon = dbcontext.HoaDons.FirstOrDefault(c => c.ID_HoaDon == id);
            var customer = dbcontext.KhachHangs.FirstOrDefault(c => c.ID_KhachHang == hoaDon.MaKhachHang);
            if (hoaDon != null && customer != null)
            {
                hoaDon.TrangThai = 2;
                customer.DiemTichLuy = customer.DiemTichLuy + Convert.ToInt32(hoaDon.TongTien) / 100;
                dbcontext.KhachHangs.Update(customer);


                dbcontext.HoaDons.Update(hoaDon);
                await dbcontext.SaveChangesAsync();

            }
            else
            {
                throw null;
            }
        }

        public async Task<HoaDon?> GetHoaDon(string id)
        {
            try
            {
                return await dbcontext.HoaDons.FirstOrDefaultAsync(c => c.ID_HoaDon == id) ?? null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<List<ViewHoaDon>> GetListHoaDon()
        {
            var listkhachhang = dbcontext.KhachHangs.ToList();
            var listhoadon = dbcontext.HoaDons.ToList();
            //var listhoadonct = dbcontext.HoaDonCTs.ToList();
            //var listsanpham = dbcontext.SanPhams.ToList();
            //var listsanphamct = dbcontext.SanPhamCTs.ToList();


            var viewhd = (from a in listkhachhang
                          join b in listhoadon on a.ID_KhachHang equals b.MaKhachHang
                          //join c in listhoadonct on b.ID_HoaDon equals c.MaHoaDon
                          //join d in listsanpham on c.MaSanPham equals d.ID_SanPham
                          //join f in listsanphamct on d.ID_SanPham equals f.MaSanPham

                          select new ViewHoaDon()
                          {
                              KhachHang = a,
                              hoaDon = b,
                              // hoaDonCT = c,
                              //sanPham = d,
                              //sanPhamCT = f,

                          }
                         ).ToList();
            var listGoupBy = viewhd.OrderBy(c => c.hoaDon.TrangThai).ToList();
            return listGoupBy;
        }

        public async Task<List<ViewHoaDonCT>> GetHDCT(string id)
        {
            var listhoadon = dbcontext.HoaDons.ToList();
            var listkhachhang = dbcontext.KhachHangs.ToList();
            var listhoadonct = dbcontext.HoaDonCTs.ToList();
            var listsanpham = dbcontext.SanPhams.ToList();
            var listsanphamct = dbcontext.SanPhamCTs.ToList();
            var listsach = dbcontext.Sachs.ToList();
            //var listsachct = dbcontext.SachCTs.ToList();
            /// var listtheloai = dbcontext.TheLoais.ToList();
            //var listtacgia = dbcontext.TacGias.ToList();
            var listnhaxuatban = dbcontext.NhaXuatBans.ToList();
            //var listphieunhap = dbcontext.PhieuNhaps.ToList();
            //var listnhanvien = dbcontext.NhanViens.ToList();

            var viewhdCT = (from a in listkhachhang
                            join b in listhoadon on a.ID_KhachHang equals b.MaKhachHang
                            join c in listhoadonct on b.ID_HoaDon equals c.MaHoaDon
                            join q in listsanpham on c.MaSanPham equals q.ID_SanPham
                            join w in listsanphamct on q.ID_SanPham equals w.MaSanPham
                            join e in listsach on w.MaSach equals e.ID_Sach
                            //join r in listsachct on e.ID_Sach equals r.MaSach
                            join h in listnhaxuatban on e.MaNXB equals h.ID_NXB

                            select new ViewHoaDonCT()
                            {
                                KhachHang = a,
                                hoaDon = b,
                                hoaDonCT = c,
                                sanPham = q,
                                sanPhamCT = w,
                                sach = e,
                                nhaXuatBan = h,

                            }

                         ).ToList();
            var HoaDonChiTiet = viewhdCT.Where(a => a.hoaDonCT.MaHoaDon == id).ToList();
            //var listhdct = viewhdCT.GroupBy(c => c.hoaDonCT.MaHoaDon==id).ToList();


            return HoaDonChiTiet;
        }

        public async Task UpdatetrangthaiHD(string id, int name)
        {


            try
            {
                var a = dbcontext.HoaDons.Where(a => a.ID_HoaDon == id).FirstOrDefault();
                a.TrangThai = name;
                dbcontext.Update(a);
                await dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Phóng code
        //Gửi mail hóa đơn
        public async Task SendMailOder(string idHoaDon)
        {
            var hoaDon = await dbcontext.HoaDons.Where(c => c.ID_HoaDon == idHoaDon).FirstOrDefaultAsync();
            var lstHoaDonCT = await dbcontext.HoaDonCTs.Where(c => c.MaHoaDon == idHoaDon).ToListAsync();
            var mess = "";



            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("phonglvph16158@fpt.edu.vn"));
            email.To.Add(MailboxAddress.Parse(hoaDon.Email));
            email.Subject = "Thông báo đơn hàng từ newbook";
            //var htmlContent = $"<h3>Đặt hàng thành công đơn hàng : {hoaDon.ID_HoaDon}  </h3>"
            //    + "<br>"
            //    + $"Tổng tiền đơn hàng {hoaDon.TongTien.ToString("#,##0").Replace(',', '.')}đ"
            //    + "<br>"
            //    + $"Tên người nhận: {hoaDon.TenNguoiNhan}"
            //         + "<br>"
            //    + $"Địa chỉ: {hoaDon.DiaChiGiaoHang}"
            //         + "<br>"
            //    + $"Số điện thoại: {hoaDon.SDT}"
            //         + "<br>"
            //    + $"Ngày mua: {hoaDon.NgayMua}"
            //         + "<br>"
            //    + $"Địa chỉ: {hoaDon.DiaChiGiaoHang}";


            if (hoaDon.TrangThai == 2)
            {
                mess = "(Đã thanh toán)";
            }



            var htmlContent = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> <html xmlns=\"http://www.w3.org/1999/xhtml\" " +
                "xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\"> <head> <!--[if gte mso 9]> <xml> <o:OfficeDocumentSettings> <o:AllowPNG/> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml> " +
                "<![endif]--> <meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"> <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"> <meta name=\"x-apple-disable-message-reformatting\"> <!--[if !mso]><!--><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><!--<![endif]--> <title></title>" +
                " <style type=\"text/css\"> @media only screen and (min-width: 660px) { .u-row { width: 640px !important; } .u-row .u-col { vertical-align: top; } .u-row .u-col-28p5 { width: 182.4px !important; } .u-row .u-col-33p33 { width: 213.31199999999998px !important; } .u-row .u-col-35p33 { width: 226.11199999999997px !important; } .u-row .u-col-38p17 { width: 244.28800000000004px !important; } " +
                ".u-row .u-col-64p67 { width: 413.88800000000003px !important; } .u-row .u-col-100 { width: 640px !important; } } @media (max-width: 660px) { .u-row-container { max-width: 100% !important; padding-left: 0px !important; padding-right: 0px !important; } .u-row .u-col { min-width: 320px !important; max-width: 100% !important; display: block !important; } .u-row { width: 100% !important; } " +
                ".u-col { width: 100% !important; } .u-col > div { margin: 0 auto; } } body { margin: 0; padding: 0; } table, tr, td { vertical-align: top; border-collapse: collapse; } p { margin: 0; } .ie-container table, .mso-container table { table-layout: fixed; } * { line-height: inherit; } a[x-apple-data-detectors='true'] { color: inherit !important; text-decoration: none !important; } table, td { color: #000000; }" +
                " #u_body a { color: #0000ee; text-decoration: underline; } </style> </head> <body class=\"clean-body u_body\" style=\"margin: 0;padding: 0;-webkit-text-size-adjust: 100%;background-color: #e7e7e7;color: #000000\"> <!--[if IE]><div class=\"ie-container\"><![endif]--> <!--[if mso]><div class=\"mso-container\"><![endif]--> <table id=\"u_body\" style=\"border-collapse: collapse;table-layout: fixed;border-spacing: 0;" +
                "mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;min-width: 320px;Margin: 0 auto;background-color: #e7e7e7;width:100%\" cellpadding=\"0\" cellspacing=\"0\"> <tbody> <tr style=\"vertical-align: top\"> <td style=\"word-break: break-word;border-collapse: collapse !important;vertical-align: top\"> <!--[if (mso)|(IE)]>" +
                "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td align=\"center\" style=\"background-color: #e7e7e7;\"><![endif]--> <div class=\"u-row-container bayengage_cart_repeat\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #2c55a5;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;" +
                "background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: #2c55a5;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"640\" style=\"width: 640px;padding: 30px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;" +
                "border-bottom: 0px solid transparent;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 640px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 30px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> " +
                "<tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"> <tr> <td style=\"padding-right: 0px;padding-left: 0px;\" align=\"center\"> <img align=\"center\" border=\"0\" src=\"https://img.bayengage.com/d189e5c42a1a/studio/41469/logo_newbook.png\" alt=\"\" title=\"\" style=\"outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 23%;max-width: 142.6px;\" width=\"142.6\"/> </td> </tr> </table> </td> </tr> </tbody> </table> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody>" +
                " <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:0px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <div style=\"line-height: 130%; text-align: left; word-wrap: break-word;\"> <p style=\"line-height: 130%; text-align: center; font-size: 14px;\"><span style=\"color: #ecf0f1; font-size: 40px; line-height: 52px;\"><span style=\"line-height: 52px; font-size: 40px;\"><strong>Đặt hàng thành công"+ mess+" ✅</strong></span></span></p> </div> </td> </tr> </tbody> </table> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <div style=\"color: #faf7f7; line-height: 140%; text-align: left; word-wrap: break-word;\"> <p style=\"font-size: 14px; line-height: 140%; text-align: center;\"><span style=\"font-size: 16px; line-height: 22.4px; font-family: arial, helvetica, sans-serif; color: #ffffff;\"><strong>ORDER ID: #"+hoaDon.ID_HoaDon +"</strong></span></p> </div> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #1e1f29;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: #1e1f29;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"640\" style=\"width: 640px;padding: 0px 20px 0px 15px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 640px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 0px 20px 0px 15px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <div style=\"color: #ffffff; line-height: 140%; text-align: left; word-wrap: break-word;\"> <p style=\"font-size: 14px; line-height: 140%;\"><span style=\"font-size: 16px; line-height: 22.4px; color: #ffffff;\"><strong>Sản phẩm</strong></span></p> </div> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container ordered_products\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ffffff;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: #ffffff;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"244\" style=\"width: 244px;padding: 12px 0px 12px 15px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-38p17\" style=\"max-width: 320px;min-width: 244.29px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 12px 0px 12px 15px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:5px 10px;font-family:arial,helvetica,sans-serif;\" align=\"left\">" +


                " <div style=\"line-height: 140%; text-align: left; word-wrap: break-word;\"> <p style=\"font-size: 14px; line-height: 140%;\"><span style=\"font-size: 14px; line-height: 19.6px;\">{{order.items.title}}</span></p> </div> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"182\" style=\"width: 182px;padding: 12px 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-28p5\" style=\"max-width: 320px;min-width: 182.4px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 12px 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:5px 10px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <div style=\"line-height: 140%; text-align: left; word-wrap: break-word;\"> <p style=\"font-size: 14px; line-height: 140%;\"><span style=\"font-size: 14px; line-height: 19.6px;\">{{order.items.quantity}}</span></p> </div> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"213\" style=\"width: 213px;padding: 12px 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-33p33\" style=\"max-width: 320px;min-width: 213.31px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 12px 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:5px 20px 5px 10px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <div style=\"line-height: 140%; text-align: right; word-wrap: break-word;\"> <p style=\"font-size: 14px; line-height: 140%;\"><span style=\"font-size: 14px; line-height: 19.6px;\">{{order.items.price}}</span></p> </div>" +


                " </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ffffff;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: #ffffff;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"640\" style=\"width: 640px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 640px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:0px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <table height=\"0px\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border-collapse: collapse;table-layout: fixed;border-spacing: 0;mso-table-lspace: 0pt;mso-table-rspace: 0pt;vertical-align: top;border-top: 1px dotted #BBBBBB;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%\"> <tbody> <tr style=\"vertical-align: top\"> <td style=\"word-break: break-word;border-collapse: collapse !important;vertical-align: top;font-size: 0px;line-height: 0px;mso-line-height-rule: exactly;-ms-text-size-adjust: 100%;-webkit-text-size-adjust: 100%\"> <span>&#160;</span> </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ffffff;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: #ffffff;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"413\" style=\"width: 413px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-64p67\" style=\"max-width: 320px;min-width: 413.89px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px 10px 10px 25px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <div style=\"line-height: 140%; text-align: left; word-wrap: break-word;\"> <p style=\"font-size: 14px; line-height: 140%; text-align: left;\"><span style=\"font-size: 14px; line-height: 19.6px;\"><strong>Tổng tiền:</strong></span></p> </div> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"226\" style=\"width: 226px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-35p33\" style=\"max-width: 320px;min-width: 226.11px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px 21px 10px 10px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <div style=\"line-height: 140%; text-align: left; word-wrap: break-word;\"> <p style=\"font-size: 14px; line-height: 140%; text-align: right;\"><span style=\"font-size: 14px; line-height: 19.6px;\"><strong>"+hoaDon.TongTien.ToString("#,##0").Replace(',', '.')+ "đ</strong></span></p> </div> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ecf0f1;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: #ecf0f1;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"600\" style=\"width: 600px;padding: 10px;border-top: 20px solid #ffffff;border-left: 20px solid #ffffff;border-right: 20px solid #ffffff;border-bottom: 20px solid #ffffff;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 640px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 10px;border-top: 20px solid #ffffff;border-left: 20px solid #ffffff;border-right: 20px solid #ffffff;border-bottom: 20px solid #ffffff;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px 10px 0px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <h4 style=\"margin: 0px; color: #000000; line-height: 140%; text-align: center; word-wrap: break-word; font-weight: normal; font-family: arial,helvetica,sans-serif; font-size: 14px;\"><strong>Thông tin :</strong></h4> </td> </tr> </tbody> </table> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:6px 10px 10px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <h3 style=\"margin: 0px; color: #000000; line-height: 140%; text-align: center; word-wrap: break-word; font-weight: normal; font-family: arial,helvetica,sans-serif; font-size: 14px;\"><p>Họ tên:"+hoaDon.TenNguoiNhan+"<br />Địa chỉ nhận hàng: "+hoaDon.DiaChiGiaoHang+"</p> <p>Số điện thoại: "+hoaDon.SDT+"</p></h3> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: #ffffff;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: #ffffff;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"640\" style=\"width: 640px;padding: 0px 20px 20px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 640px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 0px 20px 20px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <table style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px 10px 10px 0px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <div style=\"line-height: 140%; text-align: left; word-wrap: break-word;\"> <p style=\"font-size: 14px; line-height: 140%;\"><span style=\"font-size: 14px; line-height: 19.6px;\">Thank you ,</span></p> <p style=\"font-size: 14px; line-height: 140%;\"><span style=\"font-size: 14px; line-height: 19.6px;\">"+hoaDon.TenNguoiNhan+"</span></p> </div> </td> </tr> </tbody> </table> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: transparent;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"640\" style=\"width: 640px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 640px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: transparent;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"640\" style=\"width: 640px;padding: 20px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 640px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 20px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;border-radius: 0px;-webkit-border-radius: 0px; -moz-border-radius: 0px;\"><!--<![endif]--> <!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <div class=\"u-row-container\" style=\"padding: 0px;background-color: transparent\"> <div class=\"u-row\" style=\"Margin: 0 auto;min-width: 320px;max-width: 640px;overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;background-color: transparent;\"> <div style=\"border-collapse: collapse;display: table;width: 100%;height: 100%;background-color: transparent;\"> <!--[if (mso)|(IE)]><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tr><td style=\"padding: 0px;background-color: transparent;\" align=\"center\"><table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:640px;\"><tr style=\"background-color: transparent;\"><![endif]--> <!--[if (mso)|(IE)]><td align=\"center\" width=\"640\" style=\"width: 640px;padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;\" valign=\"top\"><![endif]--> <div class=\"u-col u-col-100\" style=\"max-width: 320px;min-width: 640px;display: table-cell;vertical-align: top;\"> <div style=\"height: 100%;width: 100% !important;\"> <!--[if (!mso)&(!IE)]><!--><div style=\"height: 100%; padding: 0px;border-top: 0px solid transparent;border-left: 0px solid transparent;border-right: 0px solid transparent;border-bottom: 0px solid transparent;\"><!--<![endif]--> <table class=\"logoContent\" style=\"font-family:arial,helvetica,sans-serif;\" role=\"presentation\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" border=\"0\"> <tbody> <tr> <td style=\"overflow-wrap:break-word;word-break:break-word;padding:10px;font-family:arial,helvetica,sans-serif;\" align=\"left\"> <table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"> <tr> <td style=\"padding-right: 0px;padding-left: 0px;\" align=\"center\"> <a href=\"https://localhost:7047\" target=\"_blank\"> <img align=\"center\" border=\"0\" src=\"https://img.bayengage.com/assets/1670253381339-710062.png\" alt=\"Image\" title=\"Image\" style=\"outline: none;text-decoration: none;-ms-interpolation-mode: bicubic;clear: both;display: inline-block !important;border: none;height: auto;float: none;width: 100%;max-width: 180px;\" width=\"180\"/> </a> </td> </tr> </table> </td> </tr> </tbody> </table> " +
                "<!--[if (!mso)&(!IE)]><!--></div><!--<![endif]--> </div> </div> <!--[if (mso)|(IE)]></td><![endif]--> <!--[if (mso)|(IE)]></tr></table></td></tr></table><![endif]--> </div> </div> </div> <!--[if (mso)|(IE)]></td></tr></table><![endif]--> </td> </tr> </tbody> </table> <!--[if mso]></div><![endif]--> <!--[if IE]></div><![endif]--> </body> </html>";





            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlContent };


            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("phonglvph16158@fpt.edu.vn", "Nhập mật khẩu tại đây");
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}
