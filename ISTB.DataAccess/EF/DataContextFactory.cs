using ISTB.Framework.Factories.CofigurationFactory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ISTB.DataAccess.EF
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            //throw new Exception("1");
            var configuration = new ConfigurationFactory().CreateConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("LocalConnection"));

            return new DataContext(optionsBuilder.Options);
        }
    }
}
