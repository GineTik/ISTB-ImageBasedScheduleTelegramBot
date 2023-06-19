using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Exceptions;
using ISTB.Framework.Executors.Parsers.Interfaces;

namespace ISTB.Framework.Executors.Options
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
                InvalidTypeException.ThrowIfNotImplementation<IExecutorParametersParser>(value);
                _parametersParserType = value;
            }
        }
        public string DefaultSeparator { get; set; }
        public ParseErrorMessagesAttribute ErrorMessages { get; set; } = new ParseErrorMessagesAttribute();
    }
}
