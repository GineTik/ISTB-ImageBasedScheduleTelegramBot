using Microsoft.Extensions.Configuration;

namespace ISTB.Framework.TelegramBotApplication.Factories.Interfaces
{
    public interface IConfigurationFactory
    {
        IConfiguration CreateConfiguration();
    }
}
