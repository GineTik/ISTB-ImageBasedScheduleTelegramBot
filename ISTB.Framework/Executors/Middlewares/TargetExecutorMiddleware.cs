using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Factories.Interfaces;
using ISTB.Framework.Executors.Parsers.Interfaces;
using ISTB.Framework.Executors.Storages.Interfaces;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.TelegramBotApplication.Delegates;
using ISTB.Framework.TelegramBotApplication.Middlewares;
using System.Reflection;

namespace ISTB.Framework.Executors.Middlewares
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ITargetMethodStorage _methodStorage;
        private readonly IExecutorFactory _executorFactory;
        private readonly IExecutorParametersParser _parameterParser;

        public TargetExecutorMiddleware(IExecutorParametersParser parameterParser, IExecutorFactory executorFactory,
            ITargetMethodStorage methodStorage)
        {
            _parameterParser = parameterParser;
            _executorFactory = executorFactory;
            _methodStorage = methodStorage;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            var methodInfo = _methodStorage.GetMethodInfoToExecute(updateContext.Update);

            if (methodInfo == null)
            {
                await next();
                return;
            }

            string text =
                updateContext.Update.Message?.Text ??
                updateContext.Update.CallbackQuery?.Data ??
                "";
            string separator =
                methodInfo.GetCustomAttribute<ParametersSeparatorAttribute>()?.Separator ??
                " ";
            var parameters = _parameterParser.Parse(text, methodInfo.GetParameters(), separator);

            var executorType =
                methodInfo.DeclaringType ??
                methodInfo.ReflectedType ??
                throw new InvalidOperationException($"Method {methodInfo.Name} don't have DeclaringType and ReflectedType");

            var executor = _executorFactory.CreateExecutor(executorType);
            await (Task)methodInfo.Invoke(executor, parameters);           
        }
    }
}
