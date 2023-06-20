using Microsoft.Extensions.Configuration;

namespace ISTB.Framework.TelegramBotApplication.Helpers.Factories.Configuration
{
    public interface IConfigurationFactory
    {
        IConfiguration CreateConfiguration();
    }
}
