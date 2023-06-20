using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Helpers.Factories.Implementations;
using ISTB.Framework.Executors.Helpers.Factories.Interfaces;
using ISTB.Framework.Executors.Routing.Parsers.ParameterParser;
using ISTB.Framework.Executors.Routing.Storages.TargetMethod;
using ISTB.Framework.Executors.Routing.Storages.UserState;
using ISTB.Framework.Executors.Routing.Storages.UserState.Saver;
using ISTB.Framework.Executors.Storages.Command;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ISTB.Framework.Executors.Configuration.Services
{
    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services,
            Assembly[]? assemblies = null, Action<ExecutorOptions>? configure = null)
        {
            var executorsTypes = getExecutorsTypes(assemblies);
            var executorOptions = services.configureOptions(executorsTypes, configure);

            services.addTransientServices(executorsTypes, executorOptions);
            services.addSingletonServices(executorOptions);

            return services;
        }

        private static IEnumerable<Type> getExecutorsTypes(Assembly[]? assemblies = null)
        {
            assemblies ??= new[] { Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly() };

            var baseExecutorType = typeof(Executor);
            var executorsTypes = assemblies.SelectMany(assembly =>
                assembly.GetTypes().Where(type => type != baseExecutorType && baseExecutorType.IsAssignableFrom(type))
            );

            return executorsTypes;
        }

        private static ExecutorOptions configureOptions(this IServiceCollection services,
            IEnumerable<Type> executorsTypes, Action<ExecutorOptions>? configure = null)
        {
            var executorOptions = new ExecutorOptions();
            configure?.Invoke(executorOptions);

            services.Configure<ParameterParserOptions>(options =>
            {
                options.DefaultSeparator = executorOptions.ParameterParser.DefaultSeparator;
                options.ParserType = executorOptions.ParameterParser.ParserType;
                options.ErrorMessages = executorOptions.ParameterParser.ErrorMessages;
            });

            services.Configure<UserStateOptions>(options =>
            {
                options.DefaultUserState = executorOptions.UserState.DefaultUserState;
                options.SaverType = executorOptions.UserState.SaverType;
            });

            services.Configure<TargetMethodOptinons>(options => options.ExecutorsTypes = executorsTypes);

            return executorOptions;
        }

        private static void addTransientServices(this IServiceCollection services, IEnumerable<Type> executorsTypes,
            ExecutorOptions executorOptions)
        {
            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddTransient<ICommandStorage, ExecutorCommandStorage>(); // TODO: подумати чи потрібно тут Transient чи Singleton
            services.AddTransient<IBotCommandFactory, ExecutorBotCommandFactory>();
            services.AddTransient(typeof(IParametersParser), executorOptions.ParameterParser.ParserType);

            foreach (var type in executorsTypes)
                services.AddTransient(type);
        }

        private static void addSingletonServices(this IServiceCollection services, ExecutorOptions executorOptions)
        {
            services.AddSingleton<ITargetMethodStorage, TargetMethodStorage>();
            services.AddSingleton<IUserStateStorage, UserStateStorage>();
            services.AddSingleton(typeof(IUserStateSaver), executorOptions.UserState.SaverType);
        }
    }
}
