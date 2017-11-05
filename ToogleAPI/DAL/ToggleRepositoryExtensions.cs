using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ToggleAPI.Models;

namespace ToggleAPI.DAL
{
    /// <summary>
    /// Extends the ToogleRepository class with a seeding mechanism.
    /// </summary>
    public static class ToggleRepositoryExtensions
    {
        /// <summary>
        /// Ensures that a database is created and if it is empty and, if so, it feeds some data into it.
        /// </summary>
        /// <param name="context">Database context</param>
        public static void EnsureSeedDataForContext(this ToggleContext context)
        {
            context.Database.EnsureCreated();
            if (context.ToggleItems.Any())
            {
                return;
            }
            SeedToggles(context);
        }

        public static void EnsureSeedDataWithUsers(this ToggleContext context, UserManager<SystemUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            EnsureSeedDataForContext(context);
            SeedUsers(userMgr).Wait();
        }

        private static void SeedToggles(ToggleContext context)
        {
            var isButtonBlue = new Toggle
            {
                Id = new Guid(),
                Name = "isButtonBlue",
                Version = 1,
                Configurations =
                {
                    new Configuration {SystemName = "*", Value = true}
                }
            };

            var isButtonGreen = new Toggle
            {
                Id = new Guid(),
                Name = "isButtonGreen",
                Version = 1,
                Configurations =
                {
                    new Configuration {SystemName = "Abc", Value = true}
                }
            };

            var isButtonRed = new Toggle
            {
                Id = new Guid(),
                Name = "isButtonRed",
                Version = 1,
                Configurations =
                {
                    new Configuration {SystemName = "*", Value = true},
                    new Configuration {SystemName = "Abc", Value = false}
                }
            };
            context.ToggleItems.Add(isButtonBlue);
            context.ToggleItems.Add(isButtonGreen);
            context.ToggleItems.Add(isButtonRed);

            context.SaveChanges();
        }

        private static async Task SeedUsers(UserManager<SystemUser> userMgr)
        {
            var adminUser = await userMgr.FindByNameAsync("myAdmin");
            if (adminUser == null)
            {
                var newSystemUser = await CreateNewUser(userMgr, "myAdmin", "admin@admin.com", "Administrator!01");
                await AddClaimToUser(userMgr, newSystemUser, "SuperUser", "True");
            }

            var user = await userMgr.FindByNameAsync("pedro");
            if (user == null)
            {
                await CreateNewUser(userMgr, "pedro", "pedro@pedro.com", "Password!01");
            }
        }

        private static async Task<SystemUser> CreateNewUser(UserManager<SystemUser> userMgr, string userName, string email, string password)
        {
            var user = new SystemUser()
            {
                UserName = userName,
                Email = email
            };

            var userResult = await userMgr.CreateAsync(user, password);

            if (!userResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to build user");
            }
            return user;
        }

        private static async Task AddClaimToUser(UserManager<SystemUser> userMgr, SystemUser user, string claimKey, string claimValue)
        {
            var claimResult = await userMgr.AddClaimAsync(user, new Claim(claimKey, claimValue));
            if (!claimResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to add claim to user");
            }
        }
    }
}
