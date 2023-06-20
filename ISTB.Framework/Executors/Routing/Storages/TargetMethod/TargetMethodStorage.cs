using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Executors.Configuration.Options;
using ISTB.Framework.Executors.Routing.Storages.TargetMethod.Models;
using ISTB.Framework.Executors.Routing.Storages.TargetMethod.RouteDictionaries;
using ISTB.Framework.Executors.Routing.Storages.UserState;
using ISTB.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.Options;
using System.Reflection;
using Telegram.Bot.Types.Enums;

namespace ISTB.Framework.Executors.Routing.Storages.TargetMethod
{
    public class TargetMethodStorage : ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        private readonly UpdateTypeDictionary _routes;
        private readonly IUserStateStorage _stateStorage;
        private readonly UserStateOptions _userStateOptions;

        public TargetMethodStorage(IOptions<TargetMethodOptinons> targetMethodOptions, IUserStateStorage stateStorage,
            IOptions<UserStateOptions> userStateOptions)
        {
            _stateStorage = stateStorage;
            _userStateOptions = userStateOptions.Value;

            Methods = targetMethodOptions.Value.ExecutorsTypes
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttributes<TargetAttribute>().Count() > 0)
                .Select(method => method.ReturnType == typeof(Task) ? method :
                        throw new Exception($"Return type of method {method.Name} not Task"));

            _routes = new UpdateTypeDictionary(_userStateOptions.DefaultUserState);
            _routes.AddMethods(Methods);
        }

        public async Task<MethodInfo?> GetMethodInfoToExecuteAsync(UpdateContext actualUpdateContext)
        {
            var userState = await _stateStorage.GetAsync(actualUpdateContext.TelegramUserId);

            var methods = getAppropriateMethodsToExecute(actualUpdateContext.Update.Type, userState);

            var targetMethod = methods.FirstOrDefault(method => method
                .TargetAttributes
                .Any(attr => attr.IsTarget(actualUpdateContext.Update))
            );

            return targetMethod?.MethodInfo;
        }

        private IEnumerable<TargetMethodInfo> getAppropriateMethodsToExecute(UpdateType updateType, string userState)
        {
            var methods = getAppropriateMethods(updateType, userState);

            if (updateType == UpdateType.Unknown)
                return methods;

            var unknownMethods = getAppropriateMethods(UpdateType.Unknown, userState);

            return methods.Concat(unknownMethods);
        }

        private IEnumerable<TargetMethodInfo> getAppropriateMethods(UpdateType updateType, string userState)
        {
            var states = _routes[updateType];
            states.TryGetValue(userState, out var methods);
            return methods ?? new List<TargetMethodInfo>();
        }
    }
}
