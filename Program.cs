using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartPark.Data;
using SmartPark.Models;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SmartParkContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmartParkContext") ?? throw new InvalidOperationException("Connection string 'SmartParkContext' not found.")));

// Connection string
var connectionString = builder.Configuration.GetConnectionString("SmartParkContext");
builder.Services.AddDbContext<SmartParkContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmartParkContext")));

// Dodaj Identity z vlogami
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SmartParkContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Account/Register");
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);

builder.Services.AddSwaggerGen();

// Build aplikacijo
var app = builder.Build();

app.MapControllerRoute( name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicializiraj bazo (seed)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    //await DbInitializer.SeedOverpassParking(services); // realna parkirišča iz Overpass
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
}); 

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
