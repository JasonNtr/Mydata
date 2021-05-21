using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            // Here we create the DbContextOptionsBuilder manually.        
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Build connection string. This requires that you have a connectionstring in the appsettings.json
            var connectionString = configuration.GetConnectionString("Default");
            builder.UseSqlServer(connectionString);
            // Create our DbContext.
            return new ApplicationDbContext(builder.Options);
        }

    }
}
