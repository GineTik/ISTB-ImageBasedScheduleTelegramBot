using ISTB.Framework.Executors.Factories.Implementations;
using ISTB.Framework.Executors.Factories.Interfaces;
using ISTB.Framework.Executors.Options;
using ISTB.Framework.Executors.Parsers.Implementations;
using ISTB.Framework.Executors.Parsers.Interfaces;
using ISTB.Framework.Executors.Storages.Implementations;
using ISTB.Framework.Executors.Storages.Interfaces;
using ISTB.Framework.TelegramBotApplication.Storages.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ISTB.Framework.Executors.Extensions.Services
{
    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services, 
            Assembly[]? assemblies = null, Action<ParameterParserOptions>? configure = null)
        {
            assemblies ??= new[] { Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly() };

            var baseExecutorType = typeof(Executor);
            var executorsTypes = assemblies.SelectMany(assembly =>
                assembly.GetTypes().Where(type => type != baseExecutorType && baseExecutorType.IsAssignableFrom(type))
            );

            var configureOptions = configure ?? (options =>
            {
                options.DefaultSeparator = " ";
                options.ParametersParserType = typeof(ExecutorParametersParser);
                options.ErrorMessages.TypeParseError = "Parse type error";
                options.ErrorMessages.ArgsLengthIsLess = "Args length is less";
            });

            var parserOptions = new ParameterParserOptions();
            configureOptions.Invoke(parserOptions);

            services.Configure<ParameterParserOptions>(configureOptions);

            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddTransient<ICommandStorage, ExecutorCommandStorage>(); // TODO: подумати чи потрібно тут Transient чи Singleton
            services.AddTransient<IBotCommandFactory, ExecutorBotCommandFactory>();
            services.AddTransient(typeof(IExecutorParametersParser), parserOptions.ParametersParserType);
            services.AddSingleton<ITargetMethodStorage>(new TargetMethodStorage(executorsTypes));

            foreach (var type in executorsTypes)
                services.AddTransient(type);

            return services;
        }
    }
}
