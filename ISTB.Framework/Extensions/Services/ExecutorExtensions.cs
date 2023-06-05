using ISTB.Framework.Configurations;
using ISTB.Framework.CreationalClasses.Factories.Implementations;
using ISTB.Framework.CreationalClasses.Factories.Interfaces;
using ISTB.Framework.Executors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Security.AccessControl;

namespace ISTB.Framework.Extensions.Services
{
    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services, IEnumerable<Assembly>? assemblies = null)
        {
            assemblies ??= new List<Assembly>() { Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly() };

            var baseType = typeof(Executor);
            var configuration = new ExecutorsConfiguration()
            {
                ExecutorsTypes = assemblies.SelectMany(assembly => 
                    assembly.GetTypes().Where(type => type != baseType &&
                                          baseType.IsAssignableFrom(type))
                ),
            };

            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddSingleton(configuration);

            foreach (var type in configuration.ExecutorsTypes)
                services.AddTransient(type);

            return services;
        }
    }
}
