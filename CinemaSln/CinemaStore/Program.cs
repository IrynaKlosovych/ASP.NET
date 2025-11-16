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

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(2); 
    options.Cookie.HttpOnly = true;                 
    options.Cookie.IsEssential = true;             
});

var app = builder.Build();

app.UseSession();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);

app.Run();