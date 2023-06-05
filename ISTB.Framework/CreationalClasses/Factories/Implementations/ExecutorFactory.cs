﻿using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.CreationalClasses.Factories.Interfaces;
using ISTB.Framework.Executors;
using Microsoft.Extensions.DependencyInjection;

namespace ISTB.Framework.CreationalClasses.Factories.Implementations
{
    public class ExecutorFactory : IExecutorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ExecutorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Executor CreateExecutor(Type type, UpdateContext updateContext)
        {
            ArgumentNullException.ThrowIfNull(type);
            ArgumentNullException.ThrowIfNull(updateContext);

            var baseType = typeof(Executor);
            if (type == baseType || baseType.IsAssignableFrom(type) == false)
                throw new ArgumentException(nameof(type));

            var executor = (Executor)_serviceProvider.GetRequiredService(type);
            executor.UpdateContext = updateContext;
            return executor;
        }
    }
}