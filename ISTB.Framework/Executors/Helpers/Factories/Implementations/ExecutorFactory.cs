using ISTB.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Security.AccessControl;
using ISTB.Framework.Executors.Helpers.Factories.Interfaces;

namespace ISTB.Framework.Executors.Helpers.Factories.Implementations
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

            if (isInherit<Executor>(type) == true)
                throw new ArgumentException(nameof(type));

            var executor = (Executor)_serviceProvider.GetRequiredService(type);
            executor.UpdateContext = _updateContext;
            return executor;
        }

        public T CreateExecutor<T>() where T : Executor
        {
            return (T)CreateExecutor(typeof(T));
        }

        private bool isInherit<TBase>(Type type)
        {
            var baseType = typeof(TBase);
            return type != baseType && baseType.IsAssignableFrom(type) == false;
        }
    }
}
