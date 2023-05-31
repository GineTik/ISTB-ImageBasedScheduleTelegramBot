using ISTB.Framework.Factories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ISTB.Framework.Factories.Implementations
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
