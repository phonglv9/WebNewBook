﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly SignInManager<IdentityUser> signInManager;
        private IEmailSender emailSender;
        private dbcontext _db;
        private readonly IEmailService emailService;
        private readonly IUserEmailStore<IdentityUser> emailStore;

        public LoginController(UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore, SignInManager<IdentityUser> signInManager, IEmailSender emailSender, dbcontext db, IEmailService emailService)
        {
            this.userManager = userManager;
            this.userStore = userStore;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            _db = db;
            this.emailService = emailService;
            this.emailStore = GetEmailStore();
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
            if (_db.KhachHangs.Any(c => c.Email == Input.Email) || _db.NhanViens.Any(c => c.Email == Input.Email))
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

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("hungldph13592@fpt.edu.vn"));
                email.To.Add(MailboxAddress.Parse(Input.Email));
                email.Subject = "Confirm";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>." };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("hungldph13592@fpt.edu.vn", "hungdenlolwtf");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            return Ok();
        }

        [HttpGet]
        public async Task<KhachHang> ConfirmEmail(string userId, string code, string pw, string sdt, string hoten, string diaChi, string ngaySinh)
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
            return use;
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
    }
}
