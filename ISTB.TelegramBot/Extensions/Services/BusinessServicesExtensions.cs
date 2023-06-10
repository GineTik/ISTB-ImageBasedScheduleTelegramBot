using ISTB.BusinessLogic.Services.Implementations;
using ISTB.BusinessLogic.Services.Interfaces;
using ISTB.Framework.BotApplication;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.TelegramBot.Extensions.Services
{
    public static class BusinessServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IScheduleService, ScheduleService>();
            return services;
        }
    }
}
