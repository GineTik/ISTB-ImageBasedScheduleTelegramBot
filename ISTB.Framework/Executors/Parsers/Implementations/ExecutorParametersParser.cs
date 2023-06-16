using ISTB.Framework.Executors.Extensions.Nullable;
using ISTB.Framework.Executors.Parsers.Interfaces;
using ISTB.Framework.Executors.Parsers.Results;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ISTB.Framework.Executors.Parsers.Implementations
{
    public class ExecutorParametersParser : IExecutorParametersParser
    {
        private int _missingArgs;

        public ParametersParseResult Parse(string text, ParameterInfo[] parameters, string parameterSeparator)
        {
            text = Regex.Replace(text ?? "", "^/*\\w+\\s*", "");
            
            var args = String.IsNullOrEmpty(text) ?
                new Stack<string>() :
                new Stack<string>(text.Split(parameterSeparator));
            var parametersStack = new Stack<ParameterInfo>(parameters);

            if (isCorrectArgsCount(args, parametersStack))
                return ParametersParseResult.ArgsLengthIsLess;//throw new InvalidOperationException($"Args length is less, require {parametersStack.Count - parameters.NullableCount()}");

            _missingArgs = parametersStack.Count - args.Count;
            var convertedArgs = new Stack<object?>();

            try
            {
                while (parametersStack.Count != 0)
                {
                    var value = handleBasicDataType(args, parametersStack);
                    convertedArgs.Push(value);
                }
            }
            catch
            {
                return ParametersParseResult.ParseError;
            }

            return ParametersParseResult.Success(convertedArgs.ToArray());
        }

        private bool isCorrectArgsCount(Stack<string> args, Stack<ParameterInfo> parameters)
        {
            return parameters.Count - parameters.NullableCount() > args.Count;
        }

        private object? handleBasicDataType(Stack<string> args, Stack<ParameterInfo> parameters)
        {
            var parameter = parameters.Pop();
            var targetType = parameter.ParameterType;

            if (parameter.IsNullable() == true && _missingArgs != 0)
            {
                _missingArgs--;
                return null;
            }

            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                targetType = Nullable.GetUnderlyingType(targetType)!;
            }

            var stringValue = args.Pop();

            return Convert.ChangeType(stringValue, targetType);
        }
    }
}
