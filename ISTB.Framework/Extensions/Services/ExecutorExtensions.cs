using ISTB.Framework.Configurations;
using ISTB.Framework.Executors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ISTB.Framework.Extensions.Services
{
    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services, IEnumerable<Assembly>? assemblies = null)
        {
            assemblies ??= new List<Assembly>() { Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly() };

            var configuration = new ExecutorsConfiguration()
            {
                ExecutorsTypes = assemblies.SelectMany(assembly => 
                    assembly.GetTypes().Where(type => type.BaseType == typeof(Executor))
                ),
            };

            services.AddSingleton(configuration);
            foreach (var type in configuration.ExecutorsTypes)
                services.AddTransient(type);

            return services;
        }
    }
}
