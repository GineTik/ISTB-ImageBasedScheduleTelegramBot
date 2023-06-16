using ISTB.Framework.Executors.Options;
using ISTB.Framework.Executors.Parsers.Results;

namespace ISTB.Framework.Attributes.ParametersParse
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ParseErrorMessagesAttribute : Attribute
    {
        public string ArgsLengthIsLess { get; set; }
        public string TypeParseError { get; set; }

        public string? GetActualErrorMessage(ParseErrorStatus status)
        {
            return status switch
            {
                ParseErrorStatus.ParseError => TypeParseError,
                ParseErrorStatus.ArgsLengthIsLess => ArgsLengthIsLess,
                _ => null
            };
        }
    }
}
