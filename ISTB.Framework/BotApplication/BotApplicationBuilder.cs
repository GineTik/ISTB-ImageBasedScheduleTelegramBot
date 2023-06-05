using ISTB.Framework.CreationalClasses.Factories.Implementations;
using ISTB.Framework.Extensions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.BotApplication
{
    public class BotApplicationBuilder
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

        public BotApplicationBuilder()
        {
            Services = new ServiceCollection();
            Configuration = new ConfigurationFactory().CreateConfiguration();

            Services.AddSingleton(Configuration);
            Services.AddUpdateContextAccessor();
        }

        public BotApplication Build()
        {
            return new BotApplication(this);
        }
    }
}
