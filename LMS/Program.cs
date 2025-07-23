using LMS.Data;
using LMS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

// Add Session and Cache services BEFORE building the app
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    //options.Cookie.HttpOnly = true;
    //options.Cookie.IsEssential = true;
});

// Register EmailService
builder.Services.AddTransient<EmailService>();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true; // Optional: query string keys too
});

// Build the app ONCE
var app = builder.Build();

// Use session middleware
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Dashboard/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configure Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
