using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebNewBook.API.Repository.IService;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;

namespace WebNewBook.API.Controllers
{
    [Authorize(Roles = "Admin,NhanVien")]
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

        [HttpGet("sanpham_sachct/{id}")]
        public async Task<List<SachCTViewModel>?> GetSachsBySanPhamAsync(string id)
        {
            var sachs = await sanPhamService.GetSachsBySanPhamAsync(id);
            return sachs;
        }

        [HttpGet("viewmodel")]
        public List<SanPhamViewModel> GetSanPhamViewModel()
        {
            var result = sanPhamService.GetSanPhamViewModel();
            return result.ToList();
        }

        [HttpPost]
        public async Task<ActionResult> AddSanPhamAsync(SanPhamAPI sp)
        {
            try
            {
                await sanPhamService.AddSanPhamAsync(sp);
                return Ok();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateSanPhamAsync(SanPhamAPI sp)
        {
            try
            {
                await sanPhamService.UpdateSanPhamAsync(sp.SanPham);
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

        [HttpPut("update_status")]
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
