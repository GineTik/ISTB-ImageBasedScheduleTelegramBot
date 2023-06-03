using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace ISTB.Framework.Parsers
{
    public class ParseResult
    {
        public enum ErrorTypeEnum
        {
            FewerParametersThanNeeded,
            ConvertError
        }

        public int RequiredParametersCount { get; set; }
        public ErrorTypeEnum? ErrorType { get; set; }
        public object? Value { get; set; }
        public bool IsSuccess => ErrorType == null;
    }

    public static class CommandParametersParser
    {
        public static ParseResult Parse<TParameters>(string text)
            where TParameters : new()
        {
            return Parse(typeof(TParameters), text);
        }

        public static ParseResult Parse(Type type, string text)
        {
            var args = Regex.Replace(text ?? "", "/\\w+", "");

            var propertiesInfo = type.GetProperties();
            var result = new ParseResult();
            result.RequiredParametersCount = propertiesInfo.Length;

            var parameters = args.Split(" ").ToList();
            if (propertiesInfo.Length > parameters.Count || args == "")
            {
                result.ErrorType = ParseResult.ErrorTypeEnum.FewerParametersThanNeeded;
                return result;
            }

            try
            {
                result.Value = Activator.CreateInstance(type);

                for (int i = 0; i < result.RequiredParametersCount; i++)
                {
                    var propertyType = propertiesInfo[i].PropertyType;
                    var value = Convert.ChangeType(parameters[i], propertyType);
                    propertiesInfo[i].SetValue(result.Value, value);
                }
            }
            catch
            {
                result.ErrorType = ParseResult.ErrorTypeEnum.ConvertError;
            }

            return result;
        }
    }
}
