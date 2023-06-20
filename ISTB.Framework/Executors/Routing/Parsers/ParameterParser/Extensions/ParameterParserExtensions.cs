using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Routing.Parsers.ParameterParser.Results;
using ISTB.Framework.TelegramBotApplication.Context;
using System.Reflection;

namespace ISTB.Framework.Executors.Routing.Parsers.ParameterParser.Extensions
{
    public static class ParameterParserExtensions
    {
        public static ParametersParseResult Parse(this IParametersParser parser, UpdateContext actual, MethodInfo method, ParameterParserOptions options)
        {
            var text = getTextWithArgs(actual);
            var parameters = method.GetParameters();
            var separator = getSeparator(method, options);

            return parser.Parse(
                text,
                parameters,
                separator
            );
        }

        private static string getTextWithArgs(UpdateContext updateContext)
        {
            return
                updateContext.Update.Message?.Text ??
                updateContext.Update.CallbackQuery?.Data ??
                "";
        }

        private static string getSeparator(MethodInfo methodInfo, ParameterParserOptions options)
        {
            return
                methodInfo.GetCustomAttribute<ParametersSeparatorAttribute>()?.Separator ??
                options.DefaultSeparator;
        }
    }
}
