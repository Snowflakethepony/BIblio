using Biblio.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblio.Server.Data
{
    public static class DatabaseInitializer
    {
        //TODO: Should only take in the service provider as context comes in with that aswell and extract it in the function.
        /// <summary>
        /// Initializes the database on app startup. 
        /// Also does check whether to update the database with newer migrations.
        /// NOTE:
        /// This method is not async as it has to happen syncronizely as the app must not start before these tasks are done.
        /// Also ASP.NET will cancel out awaited tasks called in Program.cs -> so it has to be done in-sync.
        /// </summary>
        /// <param name="serviceProvider">Webhost IServiceProvider</param>
        /// <param name="context">Databasecontext instance</param>
        public static void InitDb(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            while (!WaitForCon().Result) { }

            // Init the database and feed initial data if db does not exists.
            if (!context.Database.CanConnect())
            {
                // Apply initial migration to the database container.
                context.Database.Migrate();

                // Create roles -> It does not use async await to await the tasks
                // Rather Wait() method.
                CreateRoles(serviceProvider, context).Wait();

                // Seed data to database.
                SeedDB(context).Wait();
            }

            int appliedMigrations = context.Database.GetAppliedMigrations().Count();

            // Update database if needed. This also functions as "init" for azure sql and other pre-made databases.
            if (appliedMigrations < context.Database.GetMigrations().Count())
            {
                context.Database.Migrate();

                if (appliedMigrations <= 0)
                {
                    CreateRoles(serviceProvider, context).Wait();

                    // Seed data to database.
                    SeedDB(context).Wait();
                }
            }
        }

        public static async Task SeedDB(ApplicationDbContext context)
        {
            await DatabaseSeeder.SeedDatabase(context);
        }

        //TODO: Outsource the roles information to configuration files for easier management and customization.
        /// <summary>
        /// Creates ASPNETROLES for the application. Currently roles are hardcoded but will be outsourced to configuration at a later point.
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider with all build services.</param>
        /// <param name="context">Databasecontext instance</param>
        public static async Task CreateRoles(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Get role and user manager.
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Roles
            string[] roleNames = { "SiteAdministrator", "AppUser", "LibraryAdmin", "SvenstrupLibrary" };
            //IdentityResult roleResult;

            // Loop through each initial role and create it.
            foreach (var roleName in roleNames)
            {
                // Creating the roles and seeding them to the database
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //var user = await userManager.FindByNameAsync("Admin");
            // Test if admin user exists.
            if (await userManager.FindByNameAsync("Admin") == null)
            {
                // Assemble admin user.
                var adminUser = new AppUser
                {
                    UserName = "Admin@dennishk.dk",
                    Email = "Admin@dennishk.dk",
                    Firstname = "Admin",
                    EmailConfirmed = true
                };
                var userPassword = "Ponies10!";

                // Create the admin user.
                var createAdmin = await userManager.CreateAsync(adminUser, userPassword);

                // If user creation succedded add the user to admin role.
                if (createAdmin.Succeeded)
                {
                    // Assign role to user
                    await userManager.AddToRoleAsync(adminUser, "SiteAdministrator");
                }
            }
        }

        /// <summary>
        /// Async awaitable task that will internally create a child thread to run a synchronous database check function.
        /// </summary>
        /// <returns>True/ False</returns>
        public static async Task<bool> WaitForCon()
        {
            bool r = false;
            await Task.Run(() =>
            {
                r = DbCon();
            }).ConfigureAwait(false);
            return r;
        }

        /// <summary>
        /// Connectivity check of SQL Connection.
        /// </summary>
        /// <returns>True/ False</returns>
        public static bool DbCon()
        {
            using (var con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=Biblio;Trusted_Connection=True;MultipleActiveResultSets=true")) // "Data Source=library_database;User Id=sa;Password=Ponies10"// Initial Catalog=master "Data Source=library_database;User Id=sa;Password=Ponies10" "Server=tcp:library-server01.database.windows.net,1433;Initial Catalog=Collectatronica;Persist Security Info=False;User ID=Dhk;Password=Shrekislife123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
            {
                try
                {
                    con.Open();
                    return true;
                }
                catch

                {
                    return false;
                }
            }
        }
    }
}
