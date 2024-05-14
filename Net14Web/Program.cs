using Microsoft.EntityFrameworkCore;
using Net14Web.Controllers;
using Net14Web.CustomMiddlewares;
using Net14Web.DbStuff;
using Net14Web.DbStuff.Repositories;
using Net14Web.Hubs;
using Net14Web.Services;
using Net14Web.Services.ApiServices;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = AuthController.AUTH_KEY;
    })
    .AddCookie(AuthController.AUTH_KEY, option =>
    {
        option.AccessDeniedPath = "/auth/deny";
        option.LoginPath = "/Auth/Login";
    });

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.SetIsOriginAllowed(url => true);
        policy.AllowCredentials();
    });
});

builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("Net14WebDb");

builder.Services.AddDbContext<WebDbContext>(x => x.UseSqlServer(connectionString));

//builder.Services.AddScoped<WebDbContext>();

// Repositories
var typeOfBaseRepository = typeof(BaseRepository<>);
Assembly
    .GetAssembly(typeOfBaseRepository)
    .GetTypes()
    .Where(x => x.BaseType?.IsGenericType ?? false
        && x.BaseType.GetGenericTypeDefinition() == typeOfBaseRepository)
    .ToList()
    .ForEach(repositoryType => builder.Services.AddScoped(repositoryType));
//builder.Services.AddScoped<TeamRepository>();

// Services
builder.Services.AddScoped<AuthService>();



builder.Services.AddHttpClient<DateApi>(client =>
{
    client.BaseAddress = new Uri("http://numbersapi.com");
});

builder.Services.AddHttpClient<FoxApi>(client =>
{
    client.BaseAddress = new Uri("https://randomfox.ca");
});



builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCors();



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

app.UseAuthentication(); // Who I am?
app.UseAuthorization(); // May I?

app.UseMiddleware<CustomLocalizationMiddleware>();

app.MapHub<AlertHub>("/signlar-hubs/alert");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PCSHOP}/{action=Index}/{id?}");

app.Run();
