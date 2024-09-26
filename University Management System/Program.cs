using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using University_Management_System.Data;
using University_Management_System.Services;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

IConfiguration config = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Warning);

builder.Services.AddDbContext<UniversityDbContext>(options =>
{
    options.UseSqlServer(config["ConnectionString"] ?? throw new InvalidOperationException("Connection is not found."));
});

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IMenuService, MenuService>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var menuService = scope.ServiceProvider.GetRequiredService<IMenuService>();
    await menuService.ShowMenuAsync();
}


