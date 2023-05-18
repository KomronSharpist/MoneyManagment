using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoneyManagment.DAL.Contexts;
using MoneyManagment.DAL.IRepositories;
using MoneyManagment.DAL.Repositories;
using MoneyManagment.Service.Interfaces;
using MoneyManagment.Service.Mappers;
using MoneyManagment.Service.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionCategoryService, TransactionCategoryService>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddDbContext<MoneyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
