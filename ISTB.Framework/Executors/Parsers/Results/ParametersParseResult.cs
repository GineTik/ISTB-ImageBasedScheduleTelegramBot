namespace ISTB.Framework.Executors.Parsers.Results
{
    public enum ParseErrorStatus
    {
        Success,
        ParseError,
        ArgsLengthIsLess
    }

    public class ParametersParseResult
    {
        public object?[] ConvertedParameters { get; set; } = default!;
        public ParseErrorStatus Status { get; set; }

        public static ParametersParseResult Success(object?[] convertedParameters) => new ParametersParseResult
        {
            ConvertedParameters = convertedParameters,
            Status = ParseErrorStatus.Success
        };

        public static ParametersParseResult ArgsLengthIsLess => new ParametersParseResult
        {
            ConvertedParameters = new object?[0],
            Status = ParseErrorStatus.ArgsLengthIsLess
        };

        public static ParametersParseResult ParseError => new ParametersParseResult
        {
            ConvertedParameters = new object?[0],
            Status = ParseErrorStatus.ParseError
        };
    }
}
