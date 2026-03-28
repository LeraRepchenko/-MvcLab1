using Microsoft.EntityFrameworkCore;
using MvcLab1.Data;
using MvcLab1.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ========== РЕГИСТРАЦИЯ СЕРВИСОВ 
builder.Services.AddControllersWithViews();

// Регистрация контекста базы данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging());

// Регистрация репозитория 
builder.Services.AddScoped<IRecipeRepository, EfRecipeRepository>();

// ========== СБОРКА ПРИЛОЖЕНИЯ ==========
var app = builder.Build();

// ========== ИНИЦИАЛИЗАЦИЯ БАЗЫ ДАННЫХ ==========
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedData.InitializeAsync(dbContext);
}

// ========== КОНФИГУРАЦИЯ MIDDLEWARE ==========
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Кастомный маршрут 1
app.MapControllerRoute(
    name: "about",
    pattern: "about-us",
    defaults: new { controller = "Home", action = "Privacy" });

// Кастомный маршрут 2
app.MapControllerRoute(
    name: "userProfile",
    pattern: "user/{username}/{action=Profile}",
    defaults: new { controller = "Demo" });

// Маршрут с ограничениями
app.MapControllerRoute(
    name: "product",
    pattern: "product/{id:int}",
    defaults: new { controller = "Demo", action = "ProductDetails" });
//Кастомные маршрут ресторана
app.MapControllerRoute(
    name: "restaurantMenu",
    pattern: "our-menu",
    defaults: new { controller = "Restaurant", action = "Menu" });

app.Run();
