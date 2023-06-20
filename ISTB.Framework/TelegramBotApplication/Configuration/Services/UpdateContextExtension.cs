using ISTB.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.TelegramBotApplication.Configuration.Services
{
    public static class UpdateContextExtension
    {
        public static IServiceCollection AddUpdateContextAccessor(this IServiceCollection services)
        {
            return services.AddSingleton<UpdateContextAccessor>();
        }
    }
}
