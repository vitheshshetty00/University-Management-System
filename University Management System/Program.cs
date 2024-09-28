using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using University_Management_System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using University_Management_System.Services.Implementations;
using University_Management_System.Services.Interfaces;

namespace University_Management_System
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["UniversityDbContext"]?.ConnectionString
                              ?? throw new InvalidOperationException("Connection string 'UniversityDbContext' not found.");

                    CreateDatabaseIfNotExists(connectionString);

                    services.AddDbContext<UniversityDbContext>(options =>
                    {
                        options.UseSqlServer(connectionString);
                        options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) =>
                            category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Debug)));

                    });


                    services.AddScoped<IStudentService, DbStudentService>();
                    services.AddScoped<IFacultyService,DbFacultyService>();
                    services.AddScoped<ICourseService, DbCourseService>();
                    services.AddScoped<IMenuService, MenuService>();
                    services.AddScoped<IPaymentService, DbPaymentService>();
                }).Build();

            var loggerFactory = LoggerFactory.Create(builder =>
                            builder.AddConsole()
                            .AddDebug()
                            .SetMinimumLevel(LogLevel.Debug)

            );
            var logger = loggerFactory.CreateLogger<Program>();

            logger.LogInformation("Application Started");

            using var scope = host.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<UniversityDbContext>();
            dbContext.Database.Migrate();
            var menuService = scope.ServiceProvider.GetRequiredService<IMenuService>();
            await menuService.ShowMenuAsync();
        }

        private static void CreateDatabaseIfNotExists(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            string databaseName = builder.InitialCatalog;

            builder.InitialCatalog = "master";

            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"IF DB_ID(N'{databaseName}') IS NULL CREATE DATABASE [{databaseName}]", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Exception : {ex}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the database: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}