using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Parsers.ExecutorParameters.Results;
using ISTB.Framework.TelegramBotApplication.Context;
using System.Reflection;

namespace ISTB.Framework.Executors.Parsers.ExecutorParameters.Extensions
{
    public static class ParameterParserExtensions
    {
        public static ParametersParseResult Parse(this IParametersParser parser, UpdateContext actual,
            MethodInfo method, string defaultSeparator)
        {
            var text = getTextWithArgs(actual);
            var parameters = method.GetParameters();
            var separator = getSeparator(method, defaultSeparator);

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

        private static string getSeparator(MethodInfo methodInfo, string defaultSeparator)
        {
            return
                methodInfo.GetCustomAttribute<ParametersSeparatorAttribute>()?.Separator ??
                defaultSeparator;
        }
    }
}
