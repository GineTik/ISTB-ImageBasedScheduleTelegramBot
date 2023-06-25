using ISTB.BusinessLogic.Services.Implementations;
using ISTB.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.TelegramBot.Extensions.Services
{
    public static class BusinessServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IScheduleWeekService, ScheduleWeekService>();
            services.AddTransient<IScheduleDayService, ScheduleDayService>();
            return services;
        }
    }
}
