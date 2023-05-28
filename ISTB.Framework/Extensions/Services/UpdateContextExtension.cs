using ISTB.Framework.Executors.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.Extensions.Services
{
    public static class UpdateContextExtension
    {
        public static IServiceCollection AddUpdateContextAccessor(this IServiceCollection services)
        {
            return services.AddSingleton<UpdateContextAccessor>();
        }
    }
}
