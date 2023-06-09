using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.BotApplication.Delegates;
using ISTB.Framework.BotApplication.Middlewares;
using ISTB.Framework.CreationalClasses.Factories.Interfaces;
using ISTB.Framework.Executors.Storages.Interfaces;
using ISTB.Framework.Parsers.Interfaces;
using System.Reflection;

namespace ISTB.Framework.Executors.Middlewares
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ITargetMethodStorage _methodStorage;
        private readonly IExecutorFactory _executorFactory;
        private readonly IExecutorParametersParser _parameterParser;

        public TargetExecutorMiddleware(IExecutorFactory executorFactory,
            IExecutorParametersParser parameterParser, ITargetMethodStorage methodStorage)
        {
            _executorFactory = executorFactory;
            _parameterParser = parameterParser;
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

            string text = updateContext.Update.Message?.Text ?? updateContext.Update.CallbackQuery?.Data ?? "";
            string separator = methodInfo.GetCustomAttribute<ParametersSeparatorAttribute>()?.Separator ?? " ";
            var parameters = _parameterParser.Parse(text, methodInfo.GetParameters(), separator);
            
            var executorType = 
                methodInfo.DeclaringType ?? 
                methodInfo.ReflectedType ??
                throw new InvalidOperationException($"Method {methodInfo.Name} don't have DeclaringType");

            var executor = _executorFactory.CreateExecutor(executorType);
            await (Task)methodInfo.Invoke(executor, parameters);
        }
    }
}
