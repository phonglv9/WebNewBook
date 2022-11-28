
USE datn
GO




insert into HoaDon(ID_HoaDon,MaKhachHang,TenNguoiNhan,DiaChiGiaoHang,GhiChu,SDT,Email,MaGiamGia,NgayMua, TongTien, TrangThai) values ('HD11','1', 'gsyphu','hà nội','ok','90857485','email','mgg', '7/18/2022', 31, 1);
insert into HoaDon(ID_HoaDon,MaKhachHang,TenNguoiNhan,DiaChiGiaoHang,GhiChu,SDT,Email,MaGiamGia,NgayMua, TongTien, TrangThai) values ('HD21','1', 'gsyphu','hà nội','ok','90857485','email','mgg', '7/18/2022', 31, 1);
insert into HoaDon(ID_HoaDon,MaKhachHang,TenNguoiNhan,DiaChiGiaoHang,GhiChu,SDT,Email,MaGiamGia,NgayMua, TongTien, TrangThai) values ('HD32','1', 'gsyphus0','hà nội','ok','90857485','email','mgg', '7/18/2022', 31, 1);
insert into HoaDon(ID_HoaDon,MaKhachHang,TenNguoiNhan,DiaChiGiaoHang,GhiChu,SDT,Email,MaGiamGia,NgayMua, TongTien, TrangThai) values ('HD42','1', 'gsyphus0','hà nội','ok','90857485','email','mgg', '7/18/2022', 31, 1);
insert into HoaDon(ID_HoaDon,MaKhachHang,TenNguoiNhan,DiaChiGiaoHang,GhiChu,SDT,Email,MaGiamGia,NgayMua, TongTien, TrangThai) values ('HD53','1', 'gsyphus0','hà nội','ok','90857485','email','mgg','7/18/2022', 31, 1);


insert into HoaDonCT(ID_HDCT,MaSanPham,MaHoaDon,SoLuong,GiaBan) values ('HDCT21',1, 'HD11', 10,900000);
insert into HoaDonCT(ID_HDCT,MaSanPham,MaHoaDon,SoLuong,GiaBan) values ('HDCT23',2, 'HD21', 10,900000);
insert into HoaDonCT(ID_HDCT,MaSanPham,MaHoaDon,SoLuong,GiaBan) values ('HDCT34',3, 'HD32', 10,900000);
insert into HoaDonCT(ID_HDCT,MaSanPham,MaHoaDon,SoLuong,GiaBan) values ('HDCT45',4, 'HD42', 10,900000);
insert into HoaDonCT(ID_HDCT,MaSanPham,MaHoaDon,SoLuong,GiaBan) values ('HDCT56',5, 'HD53', 10,900000);






	
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (1,NULL, 'gsyphus0', 44, 31,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (2, NULL, 'sgookes1', 9, 51,'', 1);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (3, NULL, 'mjewess2', 5, 3,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (4, NULL, 'fverecker3', 25,'', 78, 1);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (5, NULL, 'hgilks4', 32, 13,'', 1);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (6, NULL, 'dpinckard5', 63, 86,'', 1);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (7, NULL, 'skeinrat6', 82, 8,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (8, NULL, 'dwimpey7', 96, 63,'', 1);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (9, NULL, 'sisley8', 88, 80,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (10, NULL, 'ahazelton9', 64, 31,'', 1);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (11, NULL, 'eholdalla', 25, 21,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (12, NULL, 'qmathiotb', 52, 87,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (13, NULL, 'ccaslakec', 28, 26,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (14, NULL, 'jfosterd', 46, 23,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (15, NULL, 'astenette', 53, 49,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (16, NULL, 'nocrianef', 35, 4,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (17, NULL, 'escarffg', 78, 98,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (18, NULL, 'cpinnerh', 28, 47,'', 2);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (19, NULL, 'sdaymenti', 50, 43,'', 1);
insert into SanPham (ID_SanPham, MaPhieuGiamGiaSP, TenSanPham, SoLuong, GiaBan,HinhAnh, TrangThai) values (20, NULL, 'bfrareyj', 91, 51,'', 2);



insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach, TrangThai) values (1, 1, 1,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (2, 2, 2,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (3, 3, 3, 0);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach, TrangThai) values (4, 4, 4,  0);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (5, 5, 5,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (6, 6, 6,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (7, 7, 7,  0);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (8, 8, 8,  0);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach, TrangThai) values (9, 9, 9,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (10, 10, 10,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (11, 11, 11,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (12, 12, 12,  0);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (13, 13, 13,  0);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (14, 14, 14,  0);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (15, 15, 15,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach, TrangThai) values (16, 16, 16,  0);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (17, 17, 17,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (18, 18, 18,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach, TrangThai) values (19, 19, 19,  1);
insert into SanPhamCT (ID_SanPhamCT, MaSanPham, MaSach,  TrangThai) values (20, 20, 20,  1);



insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (1, 1, 'Karolina Kerwick', 'VivamusIn.xls', 177, 6, 6,7, 1,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (2, 1, 'Mel Segges', 'UtMauris.mov', 130, 6, 7, 1, 89, 2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (3, 1, 'Claudell Fabry', 'Morbi.pdf', 193, 8, 7, 1, 95, 2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (4, 4, 'Estevan Knagges', 'Suscipit.tiff', 104, 2, 8, 0,7,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (5, 3, 'Vivi Gillan', 'IdOrnare.jpeg', 99, 10, 8, 1, 64, 1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (6, 2, 'Worthy Allicock', 'HabitassePlateaDictumst.avi', 64, 3, 32, 2, 1,  2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (7, 4, 'Rochella Harrad', 'Imperdiet.pdf', 149, 3, 7, 0, 25,  2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (8, 4, 'Francyne Howel', 'QuisAugueLuctus.avi', 185, 7, 1, 0, 36,  2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (9, 1, 'Fidelity Kaaskooper', 'TortorIdNulla.ppt', 149, 7, 3, 0, 83,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (10, 4, 'Graeme Brookesbie', 'NatoquePenatibus.tiff', 95, 4, 7, 0, 18, 2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (11, 2, 'Libbey Hazelden', 'TinciduntAnteVel.png', 127, 8, 1, 0, 45,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (12, 3, 'Vanya Chatainier', 'Maecenas.mp3', 164, 6, 5, 1, 91,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (13, 2, 'Reilly Winckle', 'VestibulumProinEu.mov', 117, 8, 7, 0, 46,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (14, 2, 'Bonnie Figgures', 'FelisUtAt.txt', 20, 9, 5, 1, 65,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (15, 2, 'Sibylle Ingliss', 'InHacHabitasse.xls', 51, 8, 2, 0, 32,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (16, 3, 'Wyn Irwin', 'AtVelit.xls', 199, 10, 8, 0, 97, 2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (17, 1, 'Dasie Dowber', 'UltricesPosuereCubilia.avi', 24, 32, 5, 8, 0,  1);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (18, 3, 'Chrysa Bearcock', 'Suscipit.mp3', 84, 2, 7, 0, 84,  2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (19, 4, 'Othilia Bruhke', 'EuTincidunt.xls', 147, 9, 3, 0, 32,  2);
insert into Sach (ID_Sach, MaNXB, TenSach, HinhAnh, SoTrang, TaiBan, GiaBan, MoTa, SoLuong, TrangThai) values (20, 2, 'Culver Kobke', 'Tristique.xls', 188, 8, 5, 0, 53,  2);


insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (1, 'Clerc Stanfield', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (2, 'Tabor Duding', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (3, 'Star Jewer', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (4, 'Fair Calles', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (5, 'Austine Weins', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (6, 'Candice Fleg', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (7, 'Aarika Huxstep', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (8, 'Quinton Summerside', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (9, 'Rutledge Baudino', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (10, 'Banky Blenkinship', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (11, 'Jose Vynall', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (12, 'Christyna Cordie', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (13, 'Antoinette Grishinov', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (14, 'Randie Coatman', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (15, 'Dottie Cutcliffe', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (16, 'Bernadette Jirousek', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (17, 'Kippy Cursey', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (18, 'Abagail Kingdon', 1);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (19, 'Angelina Saladino', 2);
insert into NhaXuatBan (ID_NXB, TenXuatBan, TrangThai) values (20, 'Jandy Thorley', 2);


insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (1, 8, 2, 7);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (2, 6, 18, 11);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (3, 19, 14, 20);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (4, 2, 2, 1);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (5, 16, 4, 9);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (6, 15, 10, 9);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (7, 13, 5, 5);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (8, 20, 18, 9);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (9, 14, 16, 18);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (10, 19, 7, 10);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (11, 15, 11, 11);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (12, 20, 18, 3);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (13, 14, 17, 10);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (14, 12, 20, 5);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (15, 2, 6, 15);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (16, 20, 8, 13);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (17, 2, 17, 20);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (18, 12, 11, 19);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (19, 11, 18, 8);
insert into SachCT (ID_SachCT, MaSach, MaTheLoai, MaTacGia) values (20, 6, 6, 17);


insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (1, 1, 'Vaughan Hardson',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (2, 2, 'Elbertina Blanko',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (3, 3, 'Fletch Chalk',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (4, 4, 'Lionel Soans',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (5, 1, 'Dru Ramos',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (6, 2, 'Lorain Climar',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (7, 3, 'Reggie Braden',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (8, 4, 'Mary Pirie',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (9, 1, 'Harrietta Piatto',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (10, 1, 'Duke Pinnion',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (11, 2, 'Gerda Leadbeater',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (12, 3, 'Ivan Whitlam',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (13, 4, 'Zack Latliff',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (14, 4, 'Katerina Armatage',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (15, 1, 'Tulley Munsey',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (16, 2, 'Jeanna Chalfain',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (17, 3, 'Vicki Carbry',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (18, 4, 'Nat Vernazza',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (19, 1, 'Carlin McKenzie',1);
insert into TheLoai (ID_TheLoai, MaDanhMuc, TenTL,TrangThai) values (20, 2, 'Bari Simester',1);


insert into DanhMuc (ID_DanhMuc, TenDanhMuc,TrangThai ) values (1, 'Lenee',1);
insert into DanhMuc (ID_DanhMuc, TenDanhMuc,TrangThai) values (2, 'Ryun',1);
insert into DanhMuc (ID_DanhMuc, TenDanhMuc,TrangThai) values (3, 'Georgie',1);
insert into DanhMuc (ID_DanhMuc, TenDanhMuc,TrangThai) values (4, 'Sondra',1);


insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (1,'Nguyễn Văn A ', '7/18/2022', 'Lenee', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (2,'Nguyễn Văn B ', '12/25/2021', 'Ryun', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (3,'Nguyễn Văn C ', '12/23/2021', 'Georgie', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (4,'Nguyễn Văn D ', '10/24/2021', 'Sondra', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (5, 'Nguyễn Văn E ','8/15/2022', 'Vanya', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (6, 'Nguyễn Văn G ','12/3/2021', 'Charis', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (7,'Nguyễn Văn F ', '1/10/2022', 'Arne', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (8, 'Nguyễn Văn S ','8/13/2022', 'Rebbecca', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (9, 'Nguyễn Văn J ','1/17/2022', 'Vasili', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (10,'Nguyễn Văn Q ', '6/25/2022', 'Michel', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (11,'Nguyễn Văn K ', '3/25/2022', 'Kay', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (12, 'Nguyễn Văn 1 ','4/2/2022', 'Goran', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (13, 'Nguyễn Văn 2 ','6/3/2022', 'Ag', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (14, 'Nguyễn Văn 3 ','4/2/2022', 'Babita', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (15, 'Nguyễn Văn 4 ','12/19/2021', 'Libbie', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (16, 'Nguyễn Văn 5 ','5/26/2022', 'Viviene', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (17, 'Nguyễn Văn 6 ','2/15/2022', 'Antons', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (18, 'Nguyễn Văn 7 ','10/9/2021', 'Yevette', 1);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (19, 'Nguyễn Văn A ','3/8/2022', 'Horatia', 2);
insert into TacGia (ID_TacGia,HoVaTen, NgaySinh, QueQuan, TrangThai) values (20, 'Nguyễn Văn A ','6/12/2022', 'Mala', 2);


insert into KhachHang(ID_KhachHang,HoVaTen,Email,SDT,DiaChi,NgaySinh,MatKhau,DiemTichLuy,TrangThai) values ('1', N'Lê Văn Cường', 'cuonglvph13705@fpt.edu.vn','0328326391',N'Thanh Hóa','11/24/2002','Cuonglevan123',200,1);


insert into NhanVien(ID_NhanVien, HoVaTen,Email,SDT,DiaChi,HinhAnh, NgaySinh, MatKhau, Quyen,TrangThai) values('NV01','admin', 'adminnewbook@gmail.com', '03928394039',N'Cao đẳng FPT', N'Không có','01/01/2022', '12345', 1,1);

insert into Vouchers(Id,Createdate,TenPhatHanh, StartDate,EndDate,MenhGia,MenhGiaDieuKien,GhiChu,TrangThai,MaNhanVien ) values ('PHVC01','01/01/2022', N'ngày 2010', '01/01/2022', '11/11/2022', 100000,1000000,'',1,'NV01') 

insert into VoucherCTs(Id,Createdate,MaVoucher, MaKhachHang,NgayBatDau,NgayHetHan,Diemdoi,HinhThuc,TrangThai ) values ('VC001','01/01/2022', 'PHVC01','1', '01/01/2022', '11/11/2022', 0,0,0) 


