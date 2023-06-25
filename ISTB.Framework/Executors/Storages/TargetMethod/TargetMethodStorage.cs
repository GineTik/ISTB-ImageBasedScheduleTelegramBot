using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Storages.TargetMethod.RouteDictionaries;
using ISTB.Framework.Executors.Storages.TargetMethod.StaticHelpers;
using ISTB.Framework.Executors.Storages.UserState;
using ISTB.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace ISTB.Framework.Executors.Storages.TargetMethod
{
    public sealed class TargetMethodStorage : ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        private readonly UpdateTypeDictionary _routes;
        private readonly IUserStateStorage _stateStorage;
        private readonly UserStateOptions _userStateOptions;

        public TargetMethodStorage(IOptions<TargetMethodOptinons> targetMethodOptions, IUserStateStorage stateStorage,
            IOptions<UserStateOptions> userStateOptions)
        {
            Methods = ExecutorMethodsHelper.TakeExecutorMethodsFrom(targetMethodOptions.Value.ExecutorsTypes);
            _stateStorage = stateStorage;
            _userStateOptions = userStateOptions.Value;
            _routes = new UpdateTypeDictionary(_userStateOptions.DefaultUserState);
            _routes.AddMethods(Methods);
        }

        public async Task<MethodInfo?> GetMethodInfoToExecuteAsync(UpdateContext actualUpdateContext)
        {
            var userState = await _stateStorage.GetAsync(actualUpdateContext.TelegramUserId);
            
            var methods = _routes.GetTargetMethodInfos(
                actualUpdateContext.Update.Type,
                userState
            );

            var targetMethod = methods.FirstOrDefault(method => method
                .TargetAttributes
                .Any(attr => attr.IsTarget(actualUpdateContext.Update))
            );

            return targetMethod?.MethodInfo;
        }
    }
}
