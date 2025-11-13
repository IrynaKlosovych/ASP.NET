using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CinemaStoreDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:CinemaStoreConnection"]);
});
builder.Services.AddScoped<ICinemaStoreRepository, EFStoreRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();