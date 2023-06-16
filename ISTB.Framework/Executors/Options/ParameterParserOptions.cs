using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Executors.Parsers.Interfaces;

namespace ISTB.Framework.Executors.Options
{
    public class ParameterParserOptions
    {
        private Type _parametersParserType;
        public Type ParametersParserType
        {
            get
            {
                return _parametersParserType;
            }
            set
            {
                if (value.IsInterface == true ||
                   value.IsAbstract == true ||
                   typeof(IExecutorParametersParser).IsAssignableFrom(value) == false)
                {
                    throw new ArgumentException($"Type {value.Name} is interface or abstract or not inherit IExecutorParametersParser");
                }

                _parametersParserType = value;
            }
        }
        public string DefaultSeparator { get; set; }
        public ParseErrorMessagesAttribute ErrorMessages { get; set; } = new ParseErrorMessagesAttribute();
    }
}
