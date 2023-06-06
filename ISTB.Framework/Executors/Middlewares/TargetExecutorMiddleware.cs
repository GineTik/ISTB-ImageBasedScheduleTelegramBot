using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.BotApplication.Delegates;
using ISTB.Framework.CreationalClasses.Factories.Interfaces;
using ISTB.Framework.Executors.Storages.Interfaces;
using ISTB.Framework.Parsers.Interfaces;

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
            var methodInfo = _methodStorage.GetMethodInfoByUpdate(updateContext.Update);

            if (methodInfo == null)
            {
                await next();
                return;
            }

            var executorType = methodInfo.DeclaringType ??
                throw new InvalidOperationException($"Method {methodInfo.Name} don't have DeclaringType");
            var executor = _executorFactory.CreateExecutor(executorType, updateContext);
            
            var text = updateContext.Update.Message?.Text ?? updateContext.Update.CallbackQuery?.Data ?? "";
            var parameters = _parameterParser.Parse(text, methodInfo.GetParameters());
            await (Task)methodInfo.Invoke(executor, parameters);
        }
    }
}
