using System.Reflection;

namespace ISTB.Framework.Executors.Parsers.Interfaces
{
    public interface IExecutorParametersParser
    {
        public object?[] Parse(string text, ParameterInfo[] parameters, string parameterSeparator);
    }
}
