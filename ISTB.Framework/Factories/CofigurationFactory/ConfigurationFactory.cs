using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ISTB.Framework.Factories.CofigurationFactory
{
    public class ConfigurationFactory : IConfigurationFactory
    {
        public IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Assembly.GetCallingAssembly().Location, "../../../../"))
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
