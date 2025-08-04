using deneme.Data;
using deneme.Services; // GeminiService için
using Microsoft.EntityFrameworkCore;
using Pinecone;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ► Veritabanı bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ► MVC desteği
builder.Services.AddControllersWithViews();

// AI servisini ekle
builder.Services.AddHttpClient();
builder.Services.AddScoped<IAIService, GeminiAIService>();

// ✅ GeminiService’i appsettings.json'dan API key alarak DI Container'a ekle
var apiKey = builder.Configuration["GeminiApiKey"];
builder.Services.AddSingleton(new GeminiService(apiKey));

// 3) API Controllers + JSON settings
builder.Services
    .AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    );
// 4) your embedding HTTP client
builder.Services.AddHttpClient<EmbeddingService>();

// 5) Pinecone client
builder.Services.AddSingleton(sp =>
    new PineconeClient(builder.Configuration["Pinecone:ApiKey"]!)
);

// 6) your indexing service
builder.Services.AddScoped<IndexingService>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        // CamelCase policy'yi kaldırdık - PascalCase kullanacağız
    }); // Web API Controller'lar için


// ► Uygulama oluşturuluyor
var app = builder.Build();

// ► Veritabanı migration + seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();           // otomatik migrate
    SeedData.Initialize(db);         // varsa seed data
}

// 8) Push to Pinecone
using (var scope = app.Services.CreateScope())
{
    var indexer = scope.ServiceProvider.GetRequiredService<IndexingService>();
    await indexer.UpsertAllProductsAsync();
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