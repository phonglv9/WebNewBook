using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Transactions;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;

namespace WebNewBook.API.Repository.Service
{
    public class BookService : IBookSevice
    {
        private readonly dbcontext _dbcontext;
        public BookService(dbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public class Sach_SachCT
        {
            public string TenSach { get; set; }
            public SachCT SachCT { get; set; }
            public string NXB { get; set; }
        }

        #region Sach
        public async Task<IEnumerable<Sach>> GetListSach()
        {
            try
            {
                return await _dbcontext.Sachs.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SachViewModel> GetSachViewModels()
        {
            var result = new List<SachViewModel>();
            _dbcontext.Sachs.ToList().ForEach(s =>
            {
                SachViewModel model = new SachViewModel();
                model.Sach = s;
                model.TacGia = _dbcontext.Sach_TacGias.Where(c => c.MaSach.Equals(s.ID_Sach)).Join(_dbcontext.TacGias, stg => stg.MaTacGia, tg => tg.ID_TacGia, (stg, tg) =>  tg.HoVaTen ).ToList();
                model.TacGia = _dbcontext.Sach_TheLoais.Where(c => c.MaSach.Equals(s.ID_Sach)).Join(_dbcontext.TheLoais, stl => stl.MaTheLoai, tl => tl.ID_TheLoai, (stl, tl) =>  tl.TenTL ).ToList();

                result.Add(model);
            });

            return result;
        }

        public async Task CreateSach(SachAPI input)
        {
            try
            {
                _dbcontext.Add(input.Sach);
                Add(true, input.TacGias.ToList(), input.Sach.ID_Sach);
                Add(false, input.TheLoais.ToList(), input.Sach.ID_Sach);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Đã tồn tại sách!");
            }
        }

        private void Add(bool isTacGia, List<string> lst, string id)
        {
            if (isTacGia)
            {
                foreach (var item in lst)
                {
                    Sach_TacGia sach_TacGia = new Sach_TacGia { ID_SachTacGia = "SachTG" + Guid.NewGuid().ToString(), MaTacGia = item, MaSach = id };
                    _dbcontext.Add(sach_TacGia);
                }
            }
            else
            {
                foreach (var item in lst)
                {
                    Sach_TheLoai sach_TacGia = new Sach_TheLoai { ID_SachTheLoai = "SachTG" + Guid.NewGuid().ToString(), MaTheLoai = item, MaSach = id };
                    _dbcontext.Add(sach_TacGia);
                }
            }
        }

        public async Task UpdateSach(SachAPI input)
        {
            try
            {
                _dbcontext.Update(input.Sach);
                Update(true, input.TacGias.ToList(), input.Sach.ID_Sach);
                Update(false, input.TheLoais.ToList(), input.Sach.ID_Sach);

                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Cập nhật thất bại!");
            }
        }

        private void Update(bool isTacGia, List<string> lst, string ID)
        {
            if (isTacGia)
            {
                foreach (var dbSetItem in _dbcontext.Sach_TacGias.Where(c => c.MaSach.Equals(ID)))
                {
                    if (!lst.Contains(dbSetItem.MaTacGia))
                    {
                        _dbcontext.Remove(dbSetItem);
                    }
                }

                foreach (var item in lst)
                {
                    if (!_dbcontext.Sach_TacGias.Where(c => c.MaSach.Equals(ID)).Select(c => c.MaTacGia).Contains(item))
                    {
                        var sachTG = new Sach_TacGia { ID_SachTacGia = "SachTG" + Guid.NewGuid().ToString(), MaTacGia = item, MaSach = ID };
                        _dbcontext.Add(sachTG);
                    }
                }
            }
            else
            {
                foreach (var dbSetItem in _dbcontext.Sach_TheLoais.Where(c => c.MaSach.Equals(ID)))
                {
                    if (!lst.Contains(dbSetItem.MaTheLoai))
                    {
                        _dbcontext.Remove(dbSetItem);
                    }
                }

                foreach (var item in lst)
                {
                    if (!_dbcontext.Sach_TheLoais.Where(c => c.MaSach.Equals(ID)).Select(c => c.MaTheLoai).Contains(item))
                    {
                        var sachTL = new Sach_TheLoai { ID_SachTheLoai = "SachTL" + Guid.NewGuid().ToString(), MaTheLoai = item, MaSach = ID };
                        _dbcontext.Add(sachTL);
                    }
                }
            }
        }

        public Task DeleteSach(string input)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SachCT
        public async Task CreateBook(SachCT input)
        {
            try
            {
                _dbcontext.Add(input);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Đã tồn tại sách!");
            }
        }

        public async Task DeteleBook(string ID)
        {
            try
            {
                var Model = _dbcontext.SachCTs.FirstOrDefault(c => c.ID_SachCT == ID);
                if (Model != null)
                {
                    Model.TrangThai = Model.TrangThai == 0 ? 1 : 0;
                    _dbcontext.Update(Model);
                    await _dbcontext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Đổi trạng thái thất bại!");
            }
        }

        public async Task UpdateBook(SachCT input)
        {
            try
            {
                var changePrice = _dbcontext.SachCTs.AsNoTracking().FirstOrDefault(c => c.ID_SachCT == input.ID_SachCT).GiaBan != input.GiaBan;

                _dbcontext.Update(input);
                if (changePrice)
                {
                    _dbcontext.SaveChanges();
                    var sps = new List<SanPham>();
                    var spCTs = _dbcontext.SanPhamCTs.Where(c => c.MaSachCT == input.ID_SachCT).ToList();
                    var spIds = _dbcontext.SanPhamCTs.Where(c => c.MaSachCT == input.ID_SachCT).Select(c => c.MaSanPham).ToList();
                    spIds.ForEach(c =>
                    {
                        sps.Add(_dbcontext.SanPhams.FirstOrDefault(x => x.ID_SanPham == c));
                    });

                    var data = from sp in sps
                               join spCT in _dbcontext.SanPhamCTs.ToList()
                               on sp.ID_SanPham equals spCT.MaSanPham into tempData
                               select new
                               {
                                   SPham = sp,
                                   SPCT = tempData
                               };

                    data.ToList().ForEach(c =>
                    {
                        double price = 0;
                        c.SPCT.ToList().ForEach(spct =>
                        {
                            price += _dbcontext.SachCTs.FirstOrDefault(sach => sach.ID_SachCT == spct.MaSachCT).GiaBan;
                        });

                        c.SPham.GiaBan = price * c.SPCT.FirstOrDefault().SoLuongSach;
                        c.SPham.GiaGoc = price * c.SPCT.FirstOrDefault().SoLuongSach;
                        c.SPham.TrangThai = 0;
                        _dbcontext.Update(c.SPham);
                    });
                }

                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Cập nhật thất bại!");
            }
        }

        public async Task<dynamic> GetSachTG_TL<T>(string id)
        {
            if (typeof(T) == typeof(Sach_TacGia))
            {
                return await _dbcontext.Sach_TacGias.Where(c => c.MaSach == id).ToListAsync();
            }
            return await _dbcontext.Sach_TheLoais.Where(c => c.MaSach == id).ToListAsync();
        }

        public async Task<List<SachCT>> GetSachCT()
        {
            return await _dbcontext.SachCTs.ToListAsync();
        }

        public async Task<SachCT> GetSachCT(string id)
        {
            return await _dbcontext.SachCTs.FirstOrDefaultAsync(c => c.ID_SachCT.Equals(id));
        }

        public List<SachCTViewModel> GetSachCTViewModels()
        {
            var result = _dbcontext.SachCTs.
                        Join(_dbcontext.Sachs, sachCT => sachCT.MaSach, sach => sach.ID_Sach, (sachCT, sach) => new { sachCT, sach.TenSach }).
                        Join(_dbcontext.NhaXuatBans, sachCT => sachCT.sachCT.MaNXB, nxb => nxb.ID_NXB, (sachCT, nxb) => new SachCTViewModel { SachCT = sachCT.sachCT, TenSach = sachCT.TenSach, NXB = nxb.TenXuatBan });

            return result.ToList();
        }
        #endregion

        public IEnumerable<Sach_SachCT> GetSach_SachCT()
        {
            var sachs = _dbcontext.Sachs.ToList();
            var sachcts = _dbcontext.SachCTs.Where(c => c.TrangThai == 1 && c.SoLuong > 0).ToList();
            var result = from sach in sachs join sachct in sachcts
                         on sach.ID_Sach equals sachct.MaSach join nxb in _dbcontext.NhaXuatBans on sachct.MaNXB equals nxb.ID_NXB
                         select new Sach_SachCT
                         {
                            TenSach = sach.TenSach,
                            SachCT = sachct,
                            NXB = nxb.TenXuatBan,
                         };

            return result;
        }
    }
}
