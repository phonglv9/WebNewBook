using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;

namespace WebNewBook.API.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class SanPhamController : Controller
    {
        private readonly ISanPhamService sanPhamService;
        public SanPhamController(ISanPhamService sanPhamService)
        {
            this.sanPhamService = sanPhamService;
        }

        [HttpGet]
        public async Task<IEnumerable<SanPham>> GetSanPhamAsync()
        {
            var sp = await sanPhamService.GetSanPhamAsync();
            return sp;
        }

        [HttpGet("{id}")]
        public async Task<SanPham?> GetSanPhamAsync(string id)
        {
            var sp = await sanPhamService.GetSanPhamAsync(id);
            return sp;
        }

        [HttpGet("sanpham_sach/{id}")]
        public async Task<List<Sach>?> GetSachsBySanPhamAsync(string id)
        {
            var sachs = await sanPhamService.GetSachsBySanPhamAsync(id);
            return sachs;
        }

        [HttpPost]
        public async Task<ActionResult> AddSanPhamAsync(SanPhamAPI sp)
        {
            try
            {
                await sanPhamService.AddSanPhamAsync(sp.SanPham, sp.Sachs);
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSanPhamAsync(SanPham sp)
        {
            try
            {
                await sanPhamService.UpdateSanPhamAsync(sp);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(NhanVien nhanVien, IFormFile fanh)
        //{
        //    if (fanh != null)
        //    {
        //        if (fanh != null)
        //        {
        //            string extensoin = Path.GetExtension(fanh.FileName);
        //            string image = nhanVien.HoVaTen + extensoin;
        //            nhanVien.HinhAnh = await UpLoadFile(fanh, @"SP", image.ToLower());
        //        }

        //        if (string.IsNullOrEmpty(nhanVien.HinhAnh)) nhanVien.HinhAnh = "default.jpg";
        //        var a = _context.NhanViens.ToList();
        //        nhanVien.ID_NhanVien = "NV" + a.Count;
        //        _context.Add(nhanVien);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(nhanVien);
        //}

        private static async Task<string> UpLoadFile(IFormFile file, string sDirectory, string newname)
        {
            try
            {
                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory);
                Directory.CreateDirectory(path);
                string pathfile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sDirectory, newname);
                var supportedtypes = new[] { "jpg", "jpeg", "png", "gif" };
                var fileext = Path.GetExtension(file.FileName).Substring(1);
                if (!supportedtypes.Contains(fileext.ToLower()))
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathfile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
