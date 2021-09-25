using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class DataContextSeed
    {
        public DataContextSeed()
        {
        }

        public static async Task SeedAsync(DataContext context, ILoggerFactory loggerFactory,
            UserManager<AppUser> userManager, IConfiguration configuration)
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var adminData = File.ReadAllText("../Persistence/SeedData/adminSeed.json");
                    var admin = JsonSerializer.Deserialize<List<AppUser>>(adminData);

                    foreach (var item in admin)
                    {
                        await userManager.CreateAsync(item, "Pa$$w0rd");
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<DataContext>();
                logger.LogError(e, e.Message);
            }
        }
    }
}