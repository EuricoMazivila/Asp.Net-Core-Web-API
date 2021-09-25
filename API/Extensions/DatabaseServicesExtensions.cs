using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace API.Extensions
{
    public class DatabaseSettings
    {
        public string DatabaseType { get; set; }
    }
    
    public static class DatabaseServicesExtensions
    {
        public static IConfiguration Configuration { get; set; }

        public static IServiceCollection AddDatabaseServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            Configuration = configuration;
            var databaseSection = Configuration.GetSection("Database");
            services.Configure<DatabaseSettings>(databaseSection);
            var databaseSettings = databaseSection.Get<DatabaseSettings>();

            switch (databaseSettings.DatabaseType)
            {
                case "SqlServer":
                    services.AddDbContext<DataContext, DataContextSqlServer>(ConfigureSqlServer);
                    break;
                case "MySql":
                    services.AddDbContext<DataContext, DataContextMySql>(ConfigureMySql);
                    break;
                case  "PostGreSql":
                    services.AddDbContext<DataContext, DataContextPostGreSql>(ConfigurePostGreSql);
                    break;
                default:
                    Console.WriteLine("Erro");
                    break;
            }
            
            return services;
        }

        private static void ConfigurePostGreSql(DbContextOptionsBuilder options)
        {
            string postgresSqlConnectionString = Configuration.GetConnectionString("PostGreSqlConnectionString");
            options.UseNpgsql(postgresSqlConnectionString);
            options.EnableSensitiveDataLogging();
        }

        private static void ConfigureSqlServer(DbContextOptionsBuilder options)
        {
            string sqlServerConnectionString = Configuration.GetConnectionString("SqlServerConnectionString");
            options.UseSqlServer(sqlServerConnectionString);
            options.EnableSensitiveDataLogging();
        }

        private static void ConfigureMySql(DbContextOptionsBuilder options)
        {
            string mysqlConnectionString = Configuration.GetConnectionString("MySqlConnectionString");
            options.UseMySQL(mysqlConnectionString);
            options.EnableSensitiveDataLogging();
        }
    }
}