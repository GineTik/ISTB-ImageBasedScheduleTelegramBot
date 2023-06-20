using ISTB.Framework.Executors.Routing.Parsers.ParameterParser.Results;
using System.Reflection;

namespace ISTB.Framework.Executors.Routing.Result
{
    public enum ExecuteResultStatus
    {
        Success,
        MethodNotFound,
        ParseError
    }

    public class ExecuteResult
    {
        public bool Successfully => Status == ExecuteResultStatus.Success;
        public ExecuteResultStatus Status { get; set; }
        public MethodInfo? MethodInfo { get; set; }
        public ParseStatus? ParseStatus { get; set; }

        public ExecuteResult(ExecuteResultStatus status, MethodInfo? methodInfo = null, ParseStatus? parseStatus = null)
        {
            Status = status;
            MethodInfo = methodInfo;
            ParseStatus = parseStatus;
        }

        public static ExecuteResult Success(MethodInfo method) => new ExecuteResult(ExecuteResultStatus.Success, method);
        public static ExecuteResult MethodNotFound => new ExecuteResult(ExecuteResultStatus.MethodNotFound);
        public static ExecuteResult ParseError(MethodInfo method, ParseStatus status) => new ExecuteResult(ExecuteResultStatus.ParseError, method, status);
    }
}
