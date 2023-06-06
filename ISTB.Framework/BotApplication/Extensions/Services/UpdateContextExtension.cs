using ISTB.Framework.BotApplication.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.BotApplication.Extensions.Services
{
    public static class UpdateContextExtension
    {
        public static IServiceCollection AddUpdateContextAccessor(this IServiceCollection services)
        {
            return services.AddSingleton<UpdateContextAccessor>();
        }
    }
}
