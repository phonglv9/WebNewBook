using Microsoft.EntityFrameworkCore;
using WebNewBook.API.Data;
using WebNewBook.API.Repository.IService;
using WebNewBook.API.Repository.Service;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve); ;
builder.Services.AddDbContext<dbcontext>(option => option.UseSqlServer("Data Source = 10.96.121.5; Initial Catalog = DATN; Integrated Security = False; user id = FPTShop; password = FPT$hop@^!^; Min Pool Size=20; Max Pool Size=2000; Connection Timeout = 30; Integrated Security = False; Persist Security Info=False; MultipleActiveResultSets = True; "));

builder.Services.AddControllers();
builder.Services.AddDbContext<dbcontext>(option => option.UseSqlServer("Data Source=LAPTOP-IOP6D48P\\SQLEXPRESS;Initial Catalog=FinalASM;User ID=hung;Password=hung;"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
