namespace ISTB.Framework.Results
{
    public class ParseResult : IResult
    {
        public enum ErrorTypeEnum
        {
            FewerParametersThanNeeded,
            ConvertError
        }

        public bool IsSuccess => ErrorType == null;

        public int RequiredParametersCount { get; set; }
        public ErrorTypeEnum? ErrorType { get; set; }
        public object? Value { get; set; }
    }
}
