using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.BotConfigurations
{
    public class BotApplicationBuilder
    {
        public IServiceCollection Services { get; }

        public readonly string _apiKey;

        public BotApplicationBuilder(string apiKey)
        {
            Services = new ServiceCollection();
            _apiKey = apiKey;
        }

        public BotApplication Build()
        {
            return new BotApplication(_apiKey, Services);
        }
    }
}
