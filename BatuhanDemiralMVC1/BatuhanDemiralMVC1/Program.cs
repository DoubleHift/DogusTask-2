using BatuhanDemiralMVC1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısı ekle
builder.Services.AddDbContext<UygulamaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// BlogApiVeSorgular servisini ekle
builder.Services.AddScoped<BlogApiVeSorgular>();

// Kimlik doğrulama ekle
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Hesap/Giris";
        options.LogoutPath = "/Hesap/Cikis";
        options.AccessDeniedPath = "/Hesap/ErisimEngellendi";
        options.Cookie.Name = "BlogAppAuth";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Veritabanını oluştur
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<UygulamaDbContext>();
        context.Database.EnsureCreated(); // Veritabanını oluştur
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı oluşturulurken bir hata oluştu.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Hata");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Örnek verileri ekle
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Örnek veriler eklenirken bir hata oluştu.");
    }
}

app.Run();
