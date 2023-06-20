using ISTB.Framework.Executors.Routing.Parsers.ParameterParser.Results;
using System.Reflection;

namespace ISTB.Framework.Executors.Routing.Parsers.ParameterParser
{
    public interface IParametersParser
    {
        public ParametersParseResult Parse(string text, ParameterInfo[] parameters, string parameterSeparator);
    }
}
