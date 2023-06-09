﻿using ISTB.Framework.TelegramBotApplication.Helpers.Factories.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ISTB.DataAccess.EF
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationFactory().CreateConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("LocalConnection"));

            return new DataContext(optionsBuilder.Options);
        }
    }
}
