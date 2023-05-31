using Microsoft.Extensions.Configuration;

namespace ISTB.Framework.Factories.Interfaces
{
    public interface IConfigurationFactory
    {
        IConfiguration CreateConfiguration();
    }
}
