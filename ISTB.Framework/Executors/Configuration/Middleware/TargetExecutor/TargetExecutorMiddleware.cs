using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Helpers.Factories.Interfaces;
using ISTB.Framework.Executors.Routing;
using ISTB.Framework.Executors.Routing.Parsers.ParameterParser;
using ISTB.Framework.Executors.Routing.Parsers.ParameterParser.Results;
using ISTB.Framework.Executors.Routing.Result;
using ISTB.Framework.Executors.Routing.Storages.TargetMethod;
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
        private readonly ParameterParserOptions _parameterParserOptions;
        private readonly MethodRouter _router;

        public TargetExecutorMiddleware(IOptions<ParameterParserOptions> parameterParserOptions, MethodRouter router)
        {
            _parameterParserOptions = parameterParserOptions.Value;
            _router = router;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            var result = await _router.ExecuteByCurrentUpdateAsync();

            if (result.Successfully == false)
            {
                if (result.Status == ExecuteResultStatus.ParseError)
                {
                    await parseErrorAsync(updateContext, result.MethodInfo!, result.ParseStatus!.Value);
                }

                await next();
            }
        }

        private async Task parseErrorAsync(UpdateContext updateContext, MethodInfo methodInfo, ParseStatus parseStatus)
        {
            var parseErrorMessages =
                methodInfo.GetCustomAttribute<ParseErrorMessagesAttribute>() ??
                _parameterParserOptions.ErrorMessages;

            var errorMessage = parseErrorMessages.GetActualErrorMessage(parseStatus);
            await updateContext.Client.SendTextMessageAsync(errorMessage ?? parseStatus.ToString());
        }
    }
}
