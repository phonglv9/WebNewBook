using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.PortableExecutable;
using WebNewBook.API.Common;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;

namespace WebNewBook.API.Repository.Service
{
    public class ReportService: IReportService
    {
        dbcontext _dbcontext;
        public ReportService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        //public async Task<List<ReportVM>> GetReportVMs()
        //{
        //   var _lstKhachHang = await _dbcontext.KhachHangs.ToListAsync();
        //  var  _lstHoaDon = await _dbcontext.HoaDons.ToListAsync();
        //  var  _lstHoaDonCT = await _dbcontext.HoaDonCTs.ToListAsync();
        // var   _lstSanPham = await _dbcontext.SanPhams.ToListAsync();

        //   var _lstThongKe = (from a in _lstKhachHang
        //                   join b in _lstHoaDon on a.ID_KhachHang equals b.MaKhachHang
        //                   join c in _lstHoaDonCT on b.ID_HoaDon equals c.MaHoaDon
        //                   join d in _lstSanPham on c.MaSanPham equals d.ID_SanPham
        //                   select new ReportVM()
        //                   {
        //                       khachHang = a,
        //                       hoaDon = b,
        //                       hoaDonCT = c,
        //                       sanPham = d
        //                   }).DistinctBy(c=>c.hoaDon.ID_HoaDon).Where(c=>c.hoaDon.TrangThai == 1 || c.hoaDon.TrangThai == 2 || c.hoaDon.TrangThai == 5  ).ToList();

        //    return _lstThongKe;
        //}
        public List<ReportDTO>GetReportNewBook(int type)
        {
            List<ReportDTO> list = new List<ReportDTO>();
            DataSet dataSet = TextUtils.GetDataSet("spGetReportNewBook");
            DataTable dt = new DataTable();
            if (type == 1)
            {
                 dt = dataSet.Tables[0];
            }
            if (type == 2)
            {
                 dt = dataSet.Tables[1];
            }       
             list = TextUtils.ConvertDataTable<ReportDTO>(dt);
           
            return   list;
        }
        public List<ReportDTO> GetFillterReport(string startDate,string endDate)
        {
            List<ReportDTO> list = new List<ReportDTO>();
            DataSet dataSet = TextUtils.GetDataSetSP("spGetFillterReport", new string[] { "@StartDate", "@EndDate" }, new object[] { startDate, endDate });            
             DataTable dt = dataSet.Tables[0];
            list = TextUtils.ConvertDataTable<ReportDTO>(dt);
            return list;
        }
      
    }
}
