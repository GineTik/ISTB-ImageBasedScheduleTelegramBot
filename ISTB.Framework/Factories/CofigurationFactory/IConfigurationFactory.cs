using Microsoft.Extensions.Configuration;

namespace ISTB.Framework.Factories.CofigurationFactory
{
    public interface IConfigurationFactory
    {
        IConfiguration CreateConfiguration();
    }
}
