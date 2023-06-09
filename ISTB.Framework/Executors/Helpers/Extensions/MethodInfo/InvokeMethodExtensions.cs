﻿using ISTB.Framework.Executors.Helpers.Factories.Executors;
using ISTB.Framework.Executors.Parsers.ExecutorParameters.Results;
using System.Reflection;

namespace ISTB.Framework.Executors.Helpers.Extensions.MethodInfos
{
    public static class InvokeMethodExtensions
    {
        public static async Task InvokeMethodAsync(this MethodInfo methodInfo, IExecutorFactory factory, ParametersParseResult parseResult)
        {
            var executorType = getExecutorType(methodInfo);
            var executor = factory.CreateExecutor(executorType);
            await (Task)methodInfo.Invoke(executor, parseResult.ConvertedParameters)!;
        }

        private static Type getExecutorType(MethodInfo methodInfo)
        {
            return
                methodInfo.DeclaringType ??
                methodInfo.ReflectedType ??
                throw new InvalidOperationException($"Method {methodInfo.Name} don't have DeclaringType and ReflectedType");
        }
    }
}
