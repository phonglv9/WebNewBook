using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); ;
builder.Services.AddDbContext<dbcontext>(option => option.UseSqlServer(@"Data Source=LVP-09\LVP09;Initial Catalog=WebNewBook;User ID=LVP09;Password=Ph@16158;"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IPhieuNhapService, PhieuNhapService>();
builder.Services.AddScoped<ISanPhamService, SanPhamService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
