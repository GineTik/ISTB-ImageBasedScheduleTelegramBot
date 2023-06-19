using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Executors.Options;
using ISTB.Framework.Executors.Storages.Interfaces;
using ISTB.Framework.Executors.Storages.TargetMethodRoutes;
using ISTB.Framework.Executors.Storages.UserStateSaver.Interfaces;
using ISTB.Framework.TelegramBotApplication.Context;
using Microsoft.Extensions.Options;
using System.Reflection;
using Telegram.Bot.Types.Enums;

namespace ISTB.Framework.Executors.Storages.Implementations
{
    public class TargetMethodStorage : ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        private readonly UpdateTypeDictionary _routes;
        private readonly IUserStateStorage _stateStorage;

        public TargetMethodStorage(IOptions<TargetMethodOptinons> options, IUserStateStorage stateStorage)
        {
            Methods = options.Value.ExecutorsTypes
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttributes<TargetAttribute>().Count() > 0)
                .Select(method => method.ReturnType == typeof(Task) ? method :
                        throw new Exception($"Return type of method {method.Name} not Task"));

            _routes = new UpdateTypeDictionary();
            _routes.AddMethods(Methods);

            _stateStorage = stateStorage;
        }

        public async Task<MethodInfo?> GetMethodInfoToExecuteAsync(UpdateContext updateContext)
        {
            var userState = await _stateStorage.GetAsync(updateContext.TelegramUserId);

            var methods = getMethodsToExecute(updateContext, userState);

            var targetMethod = methods.FirstOrDefault(method => method
                .TargetAttributes
                .Any(attr => attr.IsTarget(updateContext.Update))
            );

            return targetMethod?.MethodInfo;
        }

        private ICollection<TargetMethodInfo> getMethodsToExecute(UpdateContext updateContext, string userState)
        {
            var states = _routes[updateContext.Update.Type];
            states.TryGetValue(userState, out var methods);
            methods ??= new List<TargetMethodInfo>();

            if (updateContext.Update.Type == UpdateType.Unknown)
                return methods;

            var unknownStates = _routes[UpdateType.Unknown];
            unknownStates.TryGetValue(userState, out var unknownMethods);

            if (unknownMethods == null)
                return methods;

            return methods.Concat(unknownMethods).ToList();
        }
    }
}
