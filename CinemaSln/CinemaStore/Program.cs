using CinemaStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Identity;

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

builder.Services.AddDbContext<AppIdentityDbContext>
    (options => options.UseSqlServer(
        builder.Configuration["ConnectionStrings:IdentityConnection"]));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapDefaultControllerRoute();

SeedData.EnsurePopulated(app);
await SeedRolesAndAdmin(app);

app.Run();

async Task SeedRolesAndAdmin(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = config["InitialAdmin:Email"];
    var adminUserName = config["InitialAdmin:UserName"];
    var adminPassword = config["InitialAdmin:Password"];

    if (!string.IsNullOrWhiteSpace(adminEmail)
        && !string.IsNullOrWhiteSpace(adminPassword)
        && await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new IdentityUser
        {
            UserName = adminUserName ?? adminEmail,
            Email = adminEmail
        };

        var result = await userManager.CreateAsync(admin, adminPassword);

        if (result.Succeeded)
            await userManager.AddToRoleAsync(admin, "Admin");
    }
}