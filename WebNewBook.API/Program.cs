using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using Microsoft.AspNetCore.Identity;
using WebNewBook.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using static WebNewBook.API.Repository.Service.SendMailConfig;
using NETCore.MailKit.Core;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("LoginContextConnection") ?? throw new InvalidOperationException("Connection string 'LoginContextConnection' not found.");

IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
// Add services to the container.  
//
builder.Services.AddControllers(options =>
{
    options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        ReferenceHandler = ReferenceHandler.Preserve,
    }));
});
builder.Services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true);


builder.Services.AddDbContext<dbcontext>(option => option.UseSqlServer("Data Source=DESKTOP-KBU829B\\SQLEXPRESS;Initial Catalog=datn;Persist Security Info=True;User ID=sa;Password=123"));

builder.Services.AddDbContext<LoginContext>(option => option.UseSqlServer("Data Source=DESKTOP-KBU829B\\SQLEXPRESS;Initial Catalog=datn;Persist Security Info=True;User ID=sa;Password=123"));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<LoginContext>().AddDefaultTokenProviders()
    .AddDefaultUI(); ;
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<LoginContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IVoucherCTServices, VoucherCTServices>();
builder.Services.AddScoped<IPhieuNhapService, PhieuNhapService>();
builder.Services.AddScoped<ISanPhamService, SanPhamService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IBookSevice, BookService>();
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IFpointService, FpointService>();
builder.Services.AddScoped<IProfileCustomerService, ProfileCustomerService>();
builder.Services.AddScoped<IGioHangService, GioHangService>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<ITheLoaiService, TheLoaiService>();
builder.Services.AddScoped<ITacGiaService, TacGiaService>();
builder.Services.AddScoped<INhaXuatBanService, NhaXuatBanService>();
builder.Services.AddScoped<IDanhMucService, DanhMucService>();

builder.Services.AddTransient<IEmailService, SendMailConfig>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    var secret = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
    var key = new SymmetricSecurityKey(secret);
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = "https://localhost:7266/",
        ValidAudience = "https://localhost:7266/",
        IssuerSigningKey = key
    };
});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {

        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllers();

app.Run();
