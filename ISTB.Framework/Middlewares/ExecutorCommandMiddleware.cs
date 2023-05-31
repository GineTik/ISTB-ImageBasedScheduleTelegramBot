using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Configurations;
using ISTB.Framework.Delegates;
using ISTB.Framework.Factories.Interfaces;
using System.Reflection;

namespace ISTB.Framework.Middlewares
{
    public class ExecutorCommandMiddleware : IMiddleware
    {
        private readonly ExecutorsConfiguration _executorsConfiguration;
        private readonly IExecutorFactory _executorFactory;

        public ExecutorCommandMiddleware(ExecutorsConfiguration executorsConfiguration, IExecutorFactory executorFactory)
        {
            _executorsConfiguration = executorsConfiguration;
            _executorFactory = executorFactory;
        }

        public async Task InvokeAsync(UpdateContext updateContext, UpdateDelegate next)
        {
            if (updateContext.Update.Message is not { } message)
            {
                await next(updateContext);
                return;
            }

            var executorType = _executorsConfiguration.ExecutorsTypes
                .FirstOrDefault(type => type.GetCustomAttributes<TargetCommandAttribute>()
                        .Any(attribute => attribute.IsTarget(message)));

            if (executorType == null)
            {
                await next(updateContext);
                return;
            }

            var executor = _executorFactory.CreateExecutor(executorType, updateContext);
            await executor.ExecuteAsync();
        }
    }
}
