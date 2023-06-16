using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Factories.Interfaces;
using ISTB.Framework.Executors.Options;
using ISTB.Framework.Executors.Parsers.Interfaces;
using ISTB.Framework.Executors.Parsers.Results;
using ISTB.Framework.Executors.Storages.Interfaces;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.TelegramBotApplication.Delegates;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using ISTB.Framework.TelegramBotApplication.Middlewares;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ISTB.Framework.Executors.Middlewares
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ITargetMethodStorage _methodStorage;
        private readonly IExecutorFactory _executorFactory;
        private readonly IExecutorParametersParser _parameterParser;
        private readonly ParameterParserOptions _parameterParserOptions;

        public TargetExecutorMiddleware(IExecutorParametersParser parameterParser, IExecutorFactory executorFactory,
            ITargetMethodStorage methodStorage, IOptions<ParameterParserOptions> parameterParserOptions)
        {
            _parameterParser = parameterParser;
            _executorFactory = executorFactory;
            _methodStorage = methodStorage;
            _parameterParserOptions = parameterParserOptions.Value;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            var methodInfo = _methodStorage.GetMethodInfoToExecute(updateContext.Update);

            if (methodInfo == null)
            {
                await next();
                return;
            }

            var text = getTextWithArgs(updateContext);
            var parameters = methodInfo.GetParameters();
            var separator = getSeparator(methodInfo);
            var parseResult = _parameterParser.Parse(
                text,
                parameters,
                separator
            );

            if (parseResult.Status != ParseErrorStatus.Success)
            {
                await parseErrorAsync(updateContext, methodInfo, parseResult);
                return;
            }

            var executorType = getExecutorType(methodInfo);
            var executor = _executorFactory.CreateExecutor(executorType);
            await (Task)methodInfo.Invoke(executor, parseResult.ConvertedParameters)!;
        }

        private string getTextWithArgs(UpdateContext updateContext)
        {
            return
                updateContext.Update.Message?.Text ??
                updateContext.Update.CallbackQuery?.Data ??
                "";
        }

        private string getSeparator(MethodInfo methodInfo)
        {
            return 
                methodInfo.GetCustomAttribute<ParametersSeparatorAttribute>()?.Separator ??
                _parameterParserOptions.DefaultSeparator;
        }

        private async Task parseErrorAsync(UpdateContext updateContext, MethodInfo methodInfo, ParametersParseResult parseResult)
        {
            var parseErrorMessages = 
                methodInfo.GetCustomAttribute<ParseErrorMessagesAttribute>() ??
                _parameterParserOptions.ErrorMessages;

            var errorMessage = parseErrorMessages.GetActualErrorMessage(parseResult.Status);
            await updateContext.Client.SendTextMessageAsync(errorMessage ?? parseResult.Status.ToString());
        }

        private Type getExecutorType(MethodInfo methodInfo)
        {
            return 
                methodInfo.DeclaringType ??
                methodInfo.ReflectedType ??
                throw new InvalidOperationException($"Method {methodInfo.Name} don't have DeclaringType and ReflectedType");
        }
    }
}
