using ISTB.Framework.Executors.Parsers.Interfaces;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ISTB.Framework.Executors.Parsers.Implementations
{
    public class ExecutorParametersParser : IExecutorParametersParser
    {
        public object?[] Parse(string text, ParameterInfo[] ParametersInfo, string parameterSeparator)
        {
            string args = Regex.Replace(text ?? "", "^/*\\w+\\s*", "");

            if (string.IsNullOrEmpty(args))
                return new object[0];

            var stringParametersStack = new Stack<string>(args.Split(parameterSeparator));
            var resultParameters = new List<object?>();
            var nullParametersCount = ParametersInfo.Length - stringParametersStack.Count;

            if (ParametersInfo.Length - nullableCount(ParametersInfo) > stringParametersStack.Count)
                throw new InvalidOperationException($"Args length is less, require {ParametersInfo.Length - nullableCount(ParametersInfo)}");

            foreach (var parameterInfo in ParametersInfo.Reverse())
            {
                if (isNullable(parameterInfo) == true && nullParametersCount != 0)
                {
                    nullParametersCount--;
                    resultParameters.Add(null);
                    continue;
                }

                var type = parameterInfo.ParameterType;
                var value = Convert.ChangeType(stringParametersStack.Pop(), type);
                resultParameters.Add(value);
            }

            return resultParameters.Reverse<object>().ToArray();
        }

        private int nullableCount(ParameterInfo[] infos)
        {
            return infos.Count(isNullable);
        }

        private bool isNullable(ParameterInfo info)
        {
            var nullableInfoContext = new NullabilityInfoContext();
            var result = nullableInfoContext.Create(info).WriteState == NullabilityState.Nullable;
            return result;
        }
    }
}
