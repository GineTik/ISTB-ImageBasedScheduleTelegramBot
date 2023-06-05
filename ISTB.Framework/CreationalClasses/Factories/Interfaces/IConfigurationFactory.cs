using Microsoft.Extensions.Configuration;

namespace ISTB.Framework.CreationalClasses.Factories.Interfaces
{
    public interface IConfigurationFactory
    {
        IConfiguration CreateConfiguration();
    }
}
