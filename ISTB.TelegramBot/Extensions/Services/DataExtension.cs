using ISTB.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.TelegramBot.Extensions.Services
{
    public static class DataExtension
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("LocalConnection")));
            
            return services;
        }
    }
}
