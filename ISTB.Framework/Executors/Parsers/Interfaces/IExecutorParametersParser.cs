using ISTB.Framework.Executors.Parsers.Results;
using System.Reflection;

namespace ISTB.Framework.Executors.Parsers.Interfaces
{
    public interface IExecutorParametersParser
    {
        public ParametersParseResult Parse(string text, ParameterInfo[] parameters, string parameterSeparator);
    }
}
