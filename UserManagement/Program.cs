using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagement.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();

//Konfig�rasyon yap�land�rmas� i�in bir configuration nesnesi olu�turuyoruz.
IConfiguration configuration = builder.Configuration;

builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration["DbConnection"]));
builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    //�ifre kurallar�n�n olu�turulmas�
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;

    //Kullan�c�ya 5 kez deneme hakk� verir.
    options.Lockout.MaxFailedAccessAttempts = 5;
    //kullan�c�ya 5 kez denedikten sonra 5 dakika bekler
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //yeni kullan�c�lar i�in ayn� �ey ge�erli
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    //email onay� yap�ls�n
    options.SignIn.RequireConfirmedEmail = true;
    //telefon onay� yap�lmas�n
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

//Cookie konfig�rasyonu
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Security/Login";
    options.LogoutPath = "/Security/Logout";
    //Ki�inin eri�im yetkisi olmayan yere gidilmeye �al���ld���nda
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
