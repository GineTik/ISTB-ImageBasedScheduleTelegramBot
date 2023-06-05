using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.BotApplication.Delegates;
using ISTB.Framework.Configurations;
using ISTB.Framework.CreationalClasses.Factories.Interfaces;
using ISTB.Framework.Executors;
using System.Reflection;
using Telegram.Bot;

namespace ISTB.Framework.Middlewares
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ExecutorsConfiguration _executorsConfiguration;
        private readonly IExecutorFactory _executorFactory;

        public TargetExecutorMiddleware(ExecutorsConfiguration executorsConfiguration, IExecutorFactory executorFactory)
        {
            _executorsConfiguration = executorsConfiguration;
            _executorFactory = executorFactory;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            if (updateContext.Update.Message?.Text == null)
            {
                await next(updateContext);
                return;
            }

            var (executorType, methodInfo) = getExecutorTypeAndMethodByTargetAttributes(updateContext);

            if (executorType == null)
            {
                await next(updateContext);
                return;
            }
            
            // TODO: move to AddExtensions extension
            if (methodInfo.ReturnType != typeof(Task))
                throw new Exception($"Return type of method {methodInfo.Name} not Task");
            else if (methodInfo.GetParameters().Length != 0)
                throw new Exception($"Parameters of method {methodInfo.Name} is more than 0");

            var (valid, errorMessage) = await validateAsync(executorType, updateContext);
            if (valid == false)
            {
                await updateContext.Client.SendTextMessageAsync(
                    updateContext.ChatId,
                    errorMessage);
                return;
            }

            var executor = _executorFactory.CreateExecutor(executorType, updateContext);
            await (Task)methodInfo.Invoke(executor, null);
        }

        private (Type?, MethodInfo?) getExecutorTypeAndMethodByTargetAttributes(UpdateContext updateContext)
        {
            foreach (var executorType in _executorsConfiguration.ExecutorsTypes)
            {
                foreach (var methodInfo in executorType.GetMethods())
                {
                    var attributes = methodInfo
                        .GetCustomAttributes(typeof(TargetExecutorAttribute), true)
                        .Select(attrObj => attrObj as TargetExecutorAttribute);

                    if (attributes.Any(attr => attr.IsTarget(updateContext.Update)))
                    {
                        return (executorType, methodInfo);
                    }
                }
            }
            return (null, null);
        }

        private async Task<(bool, string)> validateAsync(Type executorType, UpdateContext updateContext)
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
