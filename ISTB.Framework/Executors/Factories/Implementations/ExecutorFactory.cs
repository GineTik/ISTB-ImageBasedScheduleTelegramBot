using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Factories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.Executors.Factories.Implementations
{
    public class ExecutorFactory : IExecutorFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly UpdateContext _updateContext;

        public ExecutorFactory(IServiceProvider serviceProvider, UpdateContextAccessor accessor)
        {
            _serviceProvider = serviceProvider;
            _updateContext = accessor.UpdateContext;
        }

        public Executor CreateExecutor(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            var baseType = typeof(Executor);
            if (type == baseType || baseType.IsAssignableFrom(type) == false)
                throw new ArgumentException(nameof(type));

            var executor = (Executor)_serviceProvider.GetRequiredService(type);
            executor.UpdateContext = _updateContext;
            return executor;
        }
    }
}
