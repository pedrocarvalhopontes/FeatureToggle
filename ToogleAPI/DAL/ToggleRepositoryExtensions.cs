﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToogleAPI.Models;

namespace ToogleAPI.DAL
{
    public static class ToggleRepositoryExtensions
    {
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
            IList<ToogleConfiguration> defaultConfigs = new List<ToogleConfiguration>();
            defaultConfigs.Add(new ToogleConfiguration { SystemName = "*", Value = true });

            context.ToggleItems.Add(new Toggle
            {
                Id = new Guid(),
                Name = "Default",
                Version = 1,
                Configurations = defaultConfigs
            });

            context.SaveChanges();
        }
    }
}