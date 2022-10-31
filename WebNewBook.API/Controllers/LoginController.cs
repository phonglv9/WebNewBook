using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebNewBook.API.Data;
using WebNewBook.Model.APIModels;

namespace WebNewBook.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        private dbcontext _db;

        public LoginController(dbcontext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(UserDTO userDto)
        {
            if (userDto.NhanVien)
            {
                if (_db.NhanViens.ToList().Exists(c => c.Email == userDto.Email && c.MatKhau == userDto.Password))
                {
                    var user = _db.NhanViens.FirstOrDefault(c => c.Email == userDto.Email && c.MatKhau == userDto.Password);
                    var claims = new[]
                    {
                    new Claim("Id", user.ID_NhanVien),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Quyen ? "Admin": "NhanVien")
                    };

                    return Ok(WriteJWT(claims));
                }
            }
            else
            {
                if (_db.KhachHangs.ToList().Exists(c => c.Email == userDto.Email && c.MatKhau == userDto.Password))
                {
                    var user = _db.KhachHangs.FirstOrDefault(c => c.Email == userDto.Email && c.MatKhau == userDto.Password);
                    var claims = new[]
                    {
                    new Claim("Id", user.ID_KhachHang),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "KhachHang")
                    };

                    return Ok(WriteJWT(claims));
                }
            }
            return BadRequest();
        }

        private string WriteJWT(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("https://localhost:7266/", "https://localhost:7266/", claims, expires: DateTime.Now.AddDays(1),
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
