using ISTB.Framework.Attributes.ValidateExecutionAttributes;
using ISTB.Framework.Configurations;
using ISTB.Framework.Executors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

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

        public async Task InvokeAsync(ITelegramBotClient botClient, Update update, Func<Task> next)
        {
            if (update.Message == null)
            {
                await next();
                return;
            }

            var executorType = _executorsConfiguration.ExecutorsTypes
                .FirstOrDefault(type => type.GetCustomAttributes<CommandAttribute>()
                        .Any(attribute => attribute.ValidateExecution(update.Message)));

            if (executorType == null)
            {
                await next();
                return;
            }

            var executor = (Executor)_serviceProvider.GetRequiredService(executorType);
            await executor.ExecuteAsync();
        }
    }
}
