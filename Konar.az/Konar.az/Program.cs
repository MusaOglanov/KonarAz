using Konar.az.DAL;
using Konar.az.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>(IdentityOption =>
{
    IdentityOption.Password.RequiredLength = 8;
    IdentityOption.Password.RequireUppercase = true;
    IdentityOption.Password.RequireLowercase = true;
    IdentityOption.Password.RequireNonAlphanumeric = false;
    IdentityOption.Lockout.AllowedForNewUsers = true;
    IdentityOption.Lockout.MaxFailedAccessAttempts = 3;
    IdentityOption.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    IdentityOption.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";
    IdentityOption.User.RequireUniqueEmail = true;


}).AddDefaultTokenProviders().AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider).AddEntityFrameworkStores<AppDbContext>();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
