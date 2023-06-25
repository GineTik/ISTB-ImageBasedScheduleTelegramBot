using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Helpers.Extensions.MethodInfos;
using ISTB.Framework.Executors.Helpers.Factories.Executors;
using ISTB.Framework.Executors.Parsers.ExecutorParameters;
using ISTB.Framework.Executors.Parsers.ExecutorParameters.Extensions;
using ISTB.Framework.Executors.Parsers.ExecutorParameters.Results;
using ISTB.Framework.Executors.Storages.TargetMethod;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient.Extensions;
using ISTB.Framework.TelegramBotApplication.Configuration.Middlewares;
using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.TelegramBotApplication.Delegates;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ISTB.Framework.Executors.Configuration.Middleware.TargetExecutor
{
    public class TargetExecutorMiddleware : IMiddleware
    {
        private readonly ITargetMethodStorage _methodStorage;
        private readonly IExecutorFactory _executorFactory;
        private readonly IParametersParser _parameterParser;
        private readonly ParameterParserOptions _parameterParserOptions;
        private readonly IServiceProvider _provider;

        public TargetExecutorMiddleware(IOptions<ParameterParserOptions> parameterParserOptions, IParametersParser parameterParser,
            IExecutorFactory executorFactory, ITargetMethodStorage methodStorage, IServiceProvider provider)
        {
            _parameterParserOptions = parameterParserOptions.Value;
            _parameterParser = parameterParser;
            _executorFactory = executorFactory;
            _methodStorage = methodStorage;
            _provider = provider;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            MethodInfo? methodInfo = await _methodStorage.GetMethodInfoToExecuteAsync(updateContext);

            if (methodInfo == null)
            {
                await next();
                return;
            }

            var failedValidateAttribute = methodInfo.GetCustomAttributes<ValidateInputDataAttribute>()
                .FirstOrDefault(attr => attr.ValidateAsync(updateContext, _provider).Result == false);

            if (failedValidateAttribute != null)
            {
                await updateContext.Client.SendTextMessageAsync(failedValidateAttribute.ErrorMessage);
                return;
            }

            ParametersParseResult parseResult = _parameterParser.Parse(
                updateContext,
                methodInfo,
                _parameterParserOptions.DefaultSeparator
            );

            if (parseResult.Status != ParseStatus.Success)
            {
                await handleParseErrorAsync(updateContext, methodInfo!, parseResult.Status);
                return;
            }

            await methodInfo.InvokeMethodAsync(_executorFactory, parseResult);
        }

        private async Task handleParseErrorAsync(UpdateContext updateContext, MethodInfo methodInfo, ParseStatus parseStatus)
        {
            var parseErrorMessages =
                methodInfo.GetCustomAttribute<ParseErrorMessagesAttribute>() ??
                _parameterParserOptions.ErrorMessages;

            var errorMessage = parseErrorMessages.GetActualErrorMessage(parseStatus);
            await updateContext.Client.SendTextMessageAsync(errorMessage ?? parseStatus.ToString());
        }
    }
}
