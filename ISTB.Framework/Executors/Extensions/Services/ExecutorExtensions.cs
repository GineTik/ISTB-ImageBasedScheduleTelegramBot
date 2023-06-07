using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.BotApplication.Storages.Interfaces;
using ISTB.Framework.CreationalClasses.Factories.Implementations;
using ISTB.Framework.CreationalClasses.Factories.Interfaces;
using ISTB.Framework.Executors.Storages.Implementations;
using ISTB.Framework.Executors.Storages.Interfaces;
using ISTB.Framework.Parsers.Implementations;
using ISTB.Framework.Parsers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ISTB.Framework.Executors.Extensions.Services
{
    public class ExecutorConfiguration
    {
        public IEnumerable<Assembly> Assemblies { get; set; } = 
            new List<Assembly>() { Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly() };

        public Type ParametersParserType { get; set; } = typeof(ExecutorParametersParser);

        public Type BotCommandFactoryType { get; set; } = typeof(ExecutorBotCommandFactory);
    }

    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services, Action<ExecutorConfiguration>? configureAction = null)
        {
            var configuration = configureParameters(configureAction);
            
            var baseExecutorType = typeof(Executor);
            var executorsTypes = configuration.Assemblies.SelectMany(assembly =>
                assembly.GetTypes().Where(type => type != baseExecutorType && baseExecutorType.IsAssignableFrom(type))
            );

            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddTransient<ICommandStorage, ExecutorCommandStorage>(); // TODO: подумати чи потрібно тут Transient чи Singleton
            services.AddTransient(typeof(IBotCommandFactory), configuration.BotCommandFactoryType);
            services.AddTransient(typeof(IExecutorParametersParser), configuration.ParametersParserType);
            services.AddSingleton<ITargetMethodStorage>(new TargetMethodStorage(executorsTypes));

            foreach (var type in executorsTypes)
                services.AddTransient(type);

            return services;
        }

        private static ExecutorConfiguration configureParameters(Action<ExecutorConfiguration>? configure = null)
        {
            var configuration = new ExecutorConfiguration();

            if (configure == null)
                return configuration;

            configure.Invoke(configuration);

            ArgumentNullException.ThrowIfNull(configuration.Assemblies);
            ArgumentNullException.ThrowIfNull(configuration.ParametersParserType);

            if (typeFit<IExecutorParametersParser>(configuration.ParametersParserType) == false)
                throw new ArgumentException($"{configuration.ParametersParserType.Name} is invalid");
            
            if (typeFit<IBotCommandFactory>(configuration.BotCommandFactoryType) == false)
                throw new ArgumentException($"{configuration.BotCommandFactoryType.Name} is invalid");

            return configuration;
        }

        private static bool typeFit<TBase>(Type type)
        {
            return type.IsInterface == false ||
                   type.IsAbstract == false ||
                   typeof(TBase).IsAssignableFrom(type);
        }
    }
}
