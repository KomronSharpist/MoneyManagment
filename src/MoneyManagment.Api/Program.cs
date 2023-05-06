using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyManagment.Api.Extensions;
using MoneyManagment.Api.Helpers;
using MoneyManagment.Api.Middlewares;
using MoneyManagment.Api.Models;
using MoneyManagment.DAL.Contexts;
using MoneyManagment.Service.Helpers;
using MoneyManagment.Service.Mappers;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<MoneyDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddCustomServices();

// Swagger setup
builder.Services.AddSwaggerService();

// Jwt services
builder.Services.AddJwtService(builder.Configuration);


// Logger
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Convert Api Url name to dashcase
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(
    new ConfigureApiUrlName()));
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });
});


var app = builder.Build();


EnvironmentHelper.WebHostPath =
    app.Services.GetRequiredService<IWebHostEnvironment>().WebRootPath;


// Updates db in early startup based on latest migration
app.ApplyMigrations();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseStaticFiles("/wwwroot");

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseAuthentication();

app.UseRouting();

app.MapControllers();

app.Run();