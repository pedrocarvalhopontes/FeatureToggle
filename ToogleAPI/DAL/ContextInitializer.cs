using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToogleAPI.Models;

namespace ToogleAPI.DAL
{
    public static class ContextInitializer
    {
        public static void Initialize(ToggleContext context)
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
            context.ToggleItems.Add(new Toggle
            {
                Id = 1,
                Name = "Default",
                Value = false,
                Version = 1
            });

            context.SaveChanges();
        }
    }
}
