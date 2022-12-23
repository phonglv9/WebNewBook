using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using NETCore.MailKit.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using WebNewBook.API.Data;
using WebNewBook.API.ModelsAPI;
using WebNewBook.API.Repository.Service;
using WebNewBook.Model;
using WebNewBook.Model.APIModels;
using static WebNewBook.API.Repository.Service.SendMailConfig;

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
        private readonly UserManager<IdentityUser> userManager;
        private readonly IUserStore<IdentityUser> userStore;
        private dbcontext _db;
        private readonly IEmailService emailService;
        private readonly IUserEmailStore<IdentityUser> emailStore;
        private static Random random = new Random();

        public LoginController(UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore, dbcontext db, IEmailService emailService)
        {
            this.userManager = userManager;
            this.userStore = userStore;
            _db = db;
            this.emailService = emailService;
            this.emailStore = GetEmailStore();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(UserDTO userDto)
        {
            if (userDto.NhanVien)
            {
                if (_db.NhanViens.ToList().Exists(c => c.Email == userDto.Email && c.MatKhau == userDto.Password && c.TrangThai == 1))
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
                if (_db.KhachHangs.ToList().Exists(c => c.Email == userDto.Email && c.MatKhau == userDto.Password && c.TrangThai == 1))
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
            return BadRequest("Đăng nhập thất bại!");
        }

        private string WriteJWT(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("https://localhost:7266/", "https://localhost:7266/", claims, expires: DateTime.Now.AddDays(1),
                signingCredentials: signIn);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CustomInputModel Input)
        {
            if (_db.KhachHangs.Any(c => c.Email == Input.Email) || _db.NhanViens.Any(c => c.Email == Input.Email) || await userManager.FindByEmailAsync(Input.Email) != null)
            {
                return BadRequest("Tài khoản đã tồn tại!");
            }
            string returnUrl = null;
            returnUrl ??= Url.Content("~/");
            var user = CreateUser();
            await userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                var userId = await userManager.GetUserIdAsync(user);
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                        nameof(ConfirmEmail),
                        "Login",
                        values: new { userId = userId, code = code, pw = Input.ConfirmPassword, sdt = Input.SDT, hoten = Input.HoVaTen, diaChi = Input.DiaChi, ngaySinh = Input.NgaySinh.ToString() },
                        protocol: Request.Scheme,
                        Request.Host.ToString());
                //Set gửi mail
                var email = new MimeMessage();
              email.From.Add(MailboxAddress.Parse("cuonglvph13705@fpt.edu.vn"));
                email.To.Add(MailboxAddress.Parse(Input.Email));
                email.Subject = "Confirm";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("cuonglvph13705@fpt.edu.vn", "Cuong24112002");
                smtp.Send(email);
                smtp.Disconnect(true);
                return Ok();
            }

            return BadRequest("Đăng ký thất bại, vui lòng kiểm tra lại mật khẩu đã đủ viết hoa - viết thường - số - ký tự!");
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel Input)
        {
            if (Input.NhanVien)
            {
                var user = _db.NhanViens.AsNoTracking().FirstOrDefault(c => c.ID_NhanVien == Input.ID);
                if (user.MatKhau != Input.OldPassword)
                {
                    return BadRequest("Mật khẩu cũ không hợp lệ!");
                }
                user.MatKhau = Input.Password;
                _db.Update(user);
                _db.SaveChanges();
            }
            else
            {
                var user = _db.KhachHangs.AsNoTracking().FirstOrDefault(c => c.ID_KhachHang == Input.ID);
                if (user.MatKhau != Input.OldPassword)
                {
                    return BadRequest("Mật khẩu cũ không hợp lệ!");
                }
                user.MatKhau = Input.Password;
                _db.Update(user);
                _db.SaveChanges();
            }
            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string tk)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(tk);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    return BadRequest("Không tồn tại tài khoản!");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                        nameof(ResetPassword),
                        "Login",
                        values: new {email = tk, code = code},
                        protocol: Request.Scheme,
                        Request.Host.ToString());

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("cuonglvph13705@fpt.edu.vn"));
                email.To.Add(MailboxAddress.Parse(tk));
                email.Subject = "Reset Password";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"Please click here to reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." };
                //phonglvph16158 @fpt.edu.vn", "Ph@01248460961
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("cuonglvph13705@fpt.edu.vn", "Cuong24112002");
                smtp.Send(email);
                smtp.Disconnect(true);
                return Ok();
            }

            return BadRequest("Đã có lỗi xảy ra!");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code, string pw, string sdt, string hoten, string diaChi, string ngaySinh)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null || _db.KhachHangs.Any(c => c.Email == user.Email))
            {
                return null;
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ConfirmEmailAsync(user, code);
            KhachHang use = new KhachHang();
            if (result.Succeeded)
            {
                use.ID_KhachHang = userId;
                use.Email = user.Email;
                use.MatKhau = pw;
                use.SDT = sdt;
                use.HoVaTen = hoten;
                use.DiaChi = diaChi;
                use.NgaySinh = DateTime.Parse(ngaySinh);
                use.DiemTichLuy = 0;
                use.TrangThai = 1;
                _db.Add(use);
                _db.SaveChanges();
            }
            return Redirect("https://localhost:7047/");
        }

        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email, string code)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null || !_db.KhachHangs.Any(c => c.Email == user.Email))
            {
                return NotFound();
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var pw = RandomString(10) + "@a";
            var result = await userManager.ResetPasswordAsync(user, code, pw);
            if (result.Succeeded)
            {
                var kh = _db.KhachHangs.AsNoTracking().FirstOrDefault(c => c.Email.Equals(email));
                kh.MatKhau = pw;
                _db.Update(kh);
                _db.SaveChanges();

                var emailTo = new MimeMessage();
                emailTo.From.Add(MailboxAddress.Parse("cuonglvph13705@fpt.edu.vn"));
                emailTo.To.Add(MailboxAddress.Parse(email));
                emailTo.Subject = "New Password";
                emailTo.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "Your new password is " + pw };
               
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("cuonglvph13705@fpt.edu.vn", "Cuong24112002");
                smtp.Send(emailTo);
                smtp.Disconnect(true);
            }

            return Redirect("https://localhost:7047/");
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)userStore;
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
