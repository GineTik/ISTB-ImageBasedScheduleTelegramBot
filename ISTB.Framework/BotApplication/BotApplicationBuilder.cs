using ISTB.Framework.Extensions.Services;
using ISTB.Framework.Factories.Implementations;
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
