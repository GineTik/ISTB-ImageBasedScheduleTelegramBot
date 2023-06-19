using ISTB.Framework.TelegramBotApplication.Factories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ISTB.Framework.TelegramBotApplication.Factories.Implementations
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
