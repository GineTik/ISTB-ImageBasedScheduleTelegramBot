using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Helpers.Exceptions;
using ISTB.Framework.Executors.Parsers.ExecutorParameters;

namespace ISTB.Framework.Executors.Configuration.Options
{
    public class ParameterParserOptions
    {
        private Type _parametersParserType;
        public Type ParserType
        {
            get
            {
                return _parametersParserType;
            }
            set
            {
                InvalidTypeException.ThrowIfNotImplementation<IParametersParser>(value);
                _parametersParserType = value;
            }
        }
        public string DefaultSeparator { get; set; }
        public ParseErrorMessagesAttribute ErrorMessages { get; set; } = new ParseErrorMessagesAttribute();
    }
}
