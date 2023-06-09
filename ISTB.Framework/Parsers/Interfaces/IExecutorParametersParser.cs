using System.Reflection;

namespace ISTB.Framework.Parsers.Interfaces
{
    public interface IExecutorParametersParser
    {
        public object?[] Parse(string text, ParameterInfo[] ParametersInfo);
    }
}
