using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Helpers.Factories.Interfaces;
using ISTB.Framework.Executors.Routing.Parsers.ParameterParser;
using ISTB.Framework.Executors.Routing.Parsers.ParameterParser.Extensions;
using ISTB.Framework.Executors.Routing.Parsers.ParameterParser.Results;
using ISTB.Framework.Executors.Routing.Result;
using ISTB.Framework.Executors.Routing.Storages.TargetMethod;
using ISTB.Framework.TelegramBotApplication.Context;
using System.Reflection;

namespace ISTB.Framework.Executors.Routing
{
    public class MethodRouter
    {
        private readonly ITargetMethodStorage _methodStorage;
        private readonly IExecutorFactory _executorFactory;
        private readonly IParametersParser _parameterParser;
        private readonly ParameterParserOptions _parameterParserOptions;
        private readonly UpdateContext _updateContext;

        public MethodRouter(ParameterParserOptions parameterParserOptions, IParametersParser parameterParser, 
            IExecutorFactory executorFactory, ITargetMethodStorage methodStorage, UpdateContextAccessor accessor)
        {
            _parameterParserOptions = parameterParserOptions;
            _parameterParser = parameterParser;
            _executorFactory = executorFactory;
            _methodStorage = methodStorage;
            _updateContext = accessor.UpdateContext;
        }

        public async Task<ExecuteResult> ExecuteByCurrentUpdateAsync()
        {
            var methodInfo = await _methodStorage.GetMethodInfoToExecuteAsync(_updateContext);

            if (methodInfo == null)
            {
                return ExecuteResult.MethodNotFound;
            }

            var parseResult = _parameterParser.Parse(_updateContext, methodInfo, _parameterParserOptions);

            if (parseResult.Status != ParseStatus.Success)
            {
                return ExecuteResult.ParseError(methodInfo, parseResult.Status);
            }

            await invokeMethod(methodInfo, parseResult);
            return ExecuteResult.Success(methodInfo);
        }

        private async Task invokeMethod(MethodInfo methodInfo, ParametersParseResult parseResult)
        {
            var executorType = getExecutorType(methodInfo);
            var executor = _executorFactory.CreateExecutor(executorType);
            await (Task)methodInfo.Invoke(executor, parseResult.ConvertedParameters)!;
        }

        private Type getExecutorType(MethodInfo methodInfo)
        {
            return
                methodInfo.DeclaringType ??
                methodInfo.ReflectedType ??
                throw new InvalidOperationException($"Method {methodInfo.Name} don't have DeclaringType and ReflectedType");
        }
    }
}
