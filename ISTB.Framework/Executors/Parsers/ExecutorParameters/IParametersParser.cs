using ISTB.Framework.Executors.Parsers.ExecutorParameters.Results;
using System.Reflection;

namespace ISTB.Framework.Executors.Parsers.ExecutorParameters
{
    public interface IParametersParser
    {
        public ParametersParseResult Parse(string text, ParameterInfo[] parameters, string parameterSeparator);
    }
}
