using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();

//Konfigürasyon yapýlandýrmasý için bir configuration nesnesi oluþturuyoruz.
IConfiguration configuration = builder.Configuration;

builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration["DbConnection"]));
builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    //Þifre kurallarýnýn oluþturulmasý
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;

    //Kullanýcýya 5 kez deneme hakký verir.
    options.Lockout.MaxFailedAccessAttempts = 5;
    //kullanýcýya 5 kez denedikten sonra 5 dakika bekler
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //yeni kullanýcýlar için ayný þey geçerli
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    //email onayý yapýlsýn
    options.SignIn.RequireConfirmedEmail = true;
    //telefon onayý yapýlmasýn
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

//Cookie konfigürasyonu
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Security/Login";
    options.LogoutPath = "/Security/Logout";
    //Kiþinin eriþim yetkisi olmayan yere gidilmeye çalýþýldýðýnda
    options.AccessDeniedPath = "/Security/AccessDenied";
    options.SlidingExpiration = true;
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".AspNetCoreDemo.Security.Cookie",
        Path = "/",
        SameSite = SameSiteMode.Lax,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllers();

app.Run();
