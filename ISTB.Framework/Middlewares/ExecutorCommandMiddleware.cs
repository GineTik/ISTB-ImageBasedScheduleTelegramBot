using ISTB.Framework.Attributes.ValidateExecutionAttributes;
using ISTB.Framework.Configurations;
using ISTB.Framework.Delegates;
using ISTB.Framework.Executors;
using ISTB.Framework.Executors.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ISTB.Framework.Middlewares
{
    public class ExecutorCommandMiddleware : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ExecutorsConfiguration _executorsConfiguration;

        public ExecutorCommandMiddleware(IServiceProvider serviceProvider, ExecutorsConfiguration executorsConfiguration)
        {
            _serviceProvider = serviceProvider;
            _executorsConfiguration = executorsConfiguration;
        }

        public async Task InvokeAsync(UpdateContext updateContext, UpdateDelegate next)
        {
            if (updateContext.Update.Message is not { } message)
            {
                await next(updateContext);
                return;
            }

            var executorType = _executorsConfiguration.ExecutorsTypes
                .FirstOrDefault(type => type.GetCustomAttributes<CommandAttribute>()
                        .Any(attribute => attribute.ValidateExecution(message)));

            if (executorType == null)
            {
                await next(updateContext);
                return;
            }

            var executor = (Executor)_serviceProvider.GetRequiredService(executorType);
            await executor.ExecuteAsync();
        }
    }
}
