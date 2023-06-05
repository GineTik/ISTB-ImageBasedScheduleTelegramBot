using ISTB.Framework.CreationalClasses.Factories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ISTB.Framework.CreationalClasses.Factories.Implementations
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
