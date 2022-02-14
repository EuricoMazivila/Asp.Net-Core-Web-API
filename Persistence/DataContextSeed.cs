using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                if (!context.ApplicationPermissions.Any())
                {
                    var permissionData = await File.ReadAllTextAsync("../Persistence/SeedData/permissionsSeed.json");
                    var permissions = JsonSerializer.Deserialize<List<ApplicationPermission>>(permissionData);

                    foreach (var permission in permissions)
                    {
                        await context.AddAsync(permission);
                    }
                    
                    await context.SaveChangesAsync();
                }

                if (!context.ApplicationRoles.Any())
                {
                    var rolesData = await File.ReadAllTextAsync($"../Persistence/SeedData/rolesSeed.json");
                    var roles = JsonSerializer.Deserialize<List<ApplicationRole>>(rolesData);

                    foreach (var role in roles)
                    {
                        await context.AddAsync(role);
                    }
                    
                    await context.SaveChangesAsync();
                }

                if (!context.ApplicationRolePermissions.Any())
                {
                    var rolesPermissionsData =
                        await File.ReadAllTextAsync($"../Persistence/SeedData/rolePermissionsSeed.json");
                    var rolesPermissions =
                        JsonSerializer.Deserialize<List<ApplicationRolePermissionsAux>>(rolesPermissionsData);

                    foreach (var rolePermission in rolesPermissions)
                    {
                        var role = await context.ApplicationRoles.Where(x => x.Name == rolePermission.Role)
                            .FirstOrDefaultAsync();

                        foreach (var permission in rolePermission.Permissions)
                        {
                            var appPermission = await context.ApplicationPermissions.Where(x => x.Name == permission)
                                .FirstOrDefaultAsync();
                            var appRolePermissions = new ApplicationRolePermission
                            {
                                ApplicationRole = role,
                                ApplicationPermission = appPermission
                            };
                            await context.AddAsync(appRolePermissions);
                        }
                    }
                }

                if (!userManager.Users.Any())
                {
                    var adminData = await File.ReadAllTextAsync("../Persistence/SeedData/adminSeed.json");
                    var admin = JsonSerializer.Deserialize<List<AppUser>>(adminData);

                    var roleAdmin =
                        await context.ApplicationRoles.FirstOrDefaultAsync(x => x.Name == "SystemAdministrator");
                    
                    foreach (var item in admin)
                    {
                        item.ApplicationRole = roleAdmin;
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