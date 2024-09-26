using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using University_Management_System.Data;


var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

IConfiguration config = builder.Configuration;

builder.Services.AddDbContext<UniversityContext>(Options =>
{
    Options.UseSqlServer(config["ConnectionString"] ?? throw new InvalidOperationException("Connection is not found."));
});

var host = builder.Build();

//var context = host.Services.GetRequiredService<UniversityContext>();
// await context.Database.MigrateAsync();
