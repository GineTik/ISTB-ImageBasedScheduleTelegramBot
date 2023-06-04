using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Configurations;
using ISTB.Framework.Delegates;
using ISTB.Framework.Factories.Interfaces;
using System.Reflection;
using Telegram.Bot;

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
                .FirstOrDefault(type => type.GetCustomAttributes<TargetCommandsAttribute>()
                        .Any(attr => attr.IsTarget(message)));

            if (executorType == null)
            {
                await next(updateContext);
                return;
            }

            var (valid, errorMessage) = await ValidateAsync(executorType, updateContext);
            if (valid == false)
            {
                await updateContext.Client.SendTextMessageAsync(
                    updateContext.ChatId,
                    errorMessage);
                return;
            }

            var executor = _executorFactory.CreateExecutor(executorType, updateContext);
            await executor.ExecuteAsync();
        }

        private async Task<(bool, string)> ValidateAsync(Type executorType, UpdateContext updateContext)
        {
            var validateAttributes = executorType.GetCustomAttributes<ValidateInputDataAttribute>();
            if (validateAttributes.Count() == 0)
                return (true, "");

            var valid = true;
            var errorMessage = "";
            foreach (var attr in validateAttributes)
            {
                if (await attr.ValidateAsync(updateContext) == false)
                {
                    valid = false;
                    errorMessage = attr.ErrorMessage;
                    break;
                }
            }

            return (valid, errorMessage);
        }
    }
}
