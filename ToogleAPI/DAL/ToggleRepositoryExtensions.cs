using System;
using System.Collections.Generic;
using System.Linq;
using ToogleAPI.Models;

namespace ToogleAPI.DAL
{
    /// <summary>
    /// Extends the ToogleRepository class with a seeding mechanism.
    /// </summary>
    public static class ToggleRepositoryExtensions
    {
        /// <summary>
        /// Ensures that a database is created and if it is empty and, if so, it feeds some data into it.
        /// </summary>
        /// <param name="context"></param>
        public static void EnsureSeedDataForContext(this ToggleContext context)
        {
            context.Database.EnsureCreated();
            if (context.ToggleItems.Any())
            {
                return;
            }
            Seed(context);
        }

        private static void Seed(ToggleContext context)
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
                    new Configuration {SystemName = "Abc", Value=false}
                }
            };
            context.ToggleItems.Add(isButtonBlue);
            context.ToggleItems.Add(isButtonGreen);
            context.ToggleItems.Add(isButtonRed);

            context.SaveChanges();
        }
    }
}
