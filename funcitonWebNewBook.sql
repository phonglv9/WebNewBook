USE [Webnewbook]
GO
/****** Object:  StoredProcedure [dbo].[spGetFillterReport]    Script Date: 20/12/2022 09:19:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spGetFillterReport]
	
	@StartDate datetime,
	@EndDate datetime	
AS
BEGIN

	-----Table 3 : Get Total Month chart-------
	SELECT	 CAST(DAY(h.NgayMua) as varchar) +'/' + CAST(MONTH(h.NgayMua) as varchar) +'/' +CAST( YEAR(h.NgayMua) as varchar) as DateValue ,Sum(h.TongTien) as TotalMoney,COUNT(h.ID_HoaDon) as DonHang,SUM(hdct.SoLuong) as SLSanPham
	FROM	HoaDon as h LEFT  JOIN HoaDonCT as hdct on h.ID_HoaDon = hdct.MaHoaDon
	WHERE (h.NgayMua >= @StartDate and h.NgayMua <= @EndDate)
	and (h.TrangThai = 1 or h.TrangThai = 2 or TrangThai = 5)
	GROUP BY YEAR(h.NgayMua),MONTH(h.NgayMua),DAY(h.NgayMua)

END

GO
/****** Object:  StoredProcedure [dbo].[spGetReportNewBook]    Script Date: 20/12/2022 09:19:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spGetReportNewBook]


AS
BEGIN

	-----Table 0 : Get Total year chart-------
	SELECT  CAST(MONTH(h.NgayMua) as varchar) +'/' +CAST( YEAR(h.NgayMua) as varchar) as DateValue ,SUM(h.TongTien) as TotalMoney,COUNT(h.ID_HoaDon) as DonHang,SUM(hdct.SoLuong) as SLSanPham
	FROM	HoaDon as h LEFT  JOIN HoaDonCT as hdct on h.ID_HoaDon = hdct.MaHoaDon
	WHERE	YEAR(h.NgayMua) = YEAR(GETDATE()) and h.TrangThai = 1 or h.TrangThai = 2 or TrangThai = 5	
	GROUP BY YEAR(h.NgayMua),MONTH(h.NgayMua)

	-----Table 1 : Get Total Month chart-------
	SELECT	 CAST(DAY(h.NgayMua) as varchar) +'/' + CAST(MONTH(h.NgayMua) as varchar) +'/' +CAST( YEAR(h.NgayMua) as varchar) as DateValue ,Sum(h.TongTien) as TotalMoney,COUNT(h.ID_HoaDon) as DonHang,SUM(hdct.SoLuong) as SLSanPham
	FROM	HoaDon as h LEFT  JOIN HoaDonCT as hdct on h.ID_HoaDon = hdct.MaHoaDon
	WHERE	MONTH(h.NgayMua) = MONTH(GETDATE()) and YEAR(h.NgayMua) = YEAR(GETDATE()) 	and h.TrangThai = 1 or h.TrangThai = 2 or TrangThai = 5	 
	GROUP BY YEAR(h.NgayMua),MONTH(h.NgayMua),DAY(h.NgayMua)


	-----Table 2 : Thống kê sản phẩm -----
	SELECT SP.ID_SanPham,SP.TenSanPham,SUM(HDCT.SoLuong) AS SoLuongDaBan ,SUM(HD.TongTien) AS DoanhThu
	FROM HoaDon AS HD 
		LEFT JOIN HoaDonCT AS HDCT ON HD.ID_HoaDon = HDCT.MaHoaDon 
		LEFT JOIN SanPham AS SP ON HDCT.MaSanPham = SP.ID_SanPham 
		LEFT JOIN SanPhamCT AS SPCT ON SP.ID_SanPham = SPCT.MaSanPham 
		LEFT JOIN Sach AS S ON SPCT.MaSach = S.ID_Sach
		LEFT JOIN PhieuNhap AS PM ON S.ID_Sach = PM.MaSach
	GROUP BY SP.ID_SanPham,SP.TenSanPham


	-----Table 3 : Top 10 sản phẩm bán chạy nhất -------
	SELECT  	TOP(10) sp.ID_SanPham,sp.TenSanPham ,SUM(hdct.SoLuong) AS SoLuongDaBan,SUM(HD.TongTien) AS DoanhThu
	FROM	HoaDon as hd 
			LEFT JOIN HoaDonCT AS hdct ON hd.ID_HoaDon = hdct.MaHoaDon 
			LEFT JOIN SanPham AS sp ON hdct.MaSanPham = sp.ID_SanPham
	GROUP BY sp.TenSanPham,sp.ID_SanPham
	ORDER BY SoLuongDaBan DESC




END
GO
/****** Object:  StoredProcedure [dbo].[spGetTop10ProductOder]    Script Date: 20/12/2022 09:19:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spGetTop10ProductOder]
AS
BEGIN

		-----Table 3 : Top 10 sản phẩm bán chạy nhất -------
	SELECT  	TOP(10) sp.ID_SanPham,sp.TenSanPham , sp.GiaBan,sp.GiaGoc,sp.SoLuong,sp.HinhAnh,SUM(hdct.SoLuong) AS SoLuongDaBan
	FROM	HoaDon as hd 
			LEFT JOIN HoaDonCT AS hdct ON hd.ID_HoaDon = hdct.MaHoaDon 
			LEFT JOIN SanPham AS sp ON hdct.MaSanPham = sp.ID_SanPham
	GROUP BY sp.TenSanPham,sp.ID_SanPham,sp.GiaBan,sp.GiaGoc,sp.SoLuong,sp.HinhAnh
	ORDER BY SoLuongDaBan DESC

END
GO
