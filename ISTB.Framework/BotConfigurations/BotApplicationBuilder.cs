using ISTB.Framework.Extensions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.BotConfigurations
{
    public class BotApplicationBuilder
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }

        public BotApplicationBuilder()
        {
            Services = new ServiceCollection();
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            Services.AddSingleton(Configuration);
            Services.AddExecutorContextAccessor();
        }

        public BotApplication Build()
        {
            return new BotApplication(this);
        }
    }
}
