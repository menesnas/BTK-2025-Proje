using deneme.Data;
using deneme.Services; // GeminiService için
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ► Veritabanı bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ► MVC desteği
builder.Services.AddControllersWithViews();

// ✅ GeminiService’i appsettings.json'dan API key alarak DI Container'a ekle
var apiKey = builder.Configuration["GeminiApiKey"];
builder.Services.AddSingleton(new GeminiService(apiKey));


// ► Uygulama oluşturuluyor
var app = builder.Build();

// ► Veritabanı migration + seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();           // otomatik migrate
    SeedData.Initialize(db);         // varsa seed data
}

// ► Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// ► Route tanımı
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();