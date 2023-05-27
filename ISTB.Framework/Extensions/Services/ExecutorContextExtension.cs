using ISTB.Framework.Executors.Context;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.Extensions.Services
{
    public static class ExecutorContextExtension
    {
        public static IServiceCollection AddExecutorContextAccessor(this IServiceCollection services)
        {
            return services.AddSingleton<ExecutorContextAccessor>();
        }
    }
}
