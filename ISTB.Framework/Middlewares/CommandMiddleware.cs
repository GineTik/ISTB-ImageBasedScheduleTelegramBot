using ISTB.Framework.Attributes.ValidateExecutionAttributes;
using ISTB.Framework.Executors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ISTB.Framework.Middlewares
{
    public class CommandMiddleware : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandMiddleware(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task InvokeAsync(ITelegramBotClient botClient, Update update, Func<Task> next)
        {
            if (update.Message == null)
            {
                await next();
                return;
            }

            var executorType = Assembly.GetEntryAssembly().GetTypes()
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
