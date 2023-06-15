using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Executors.Helpers;
using ISTB.Framework.Executors.Storages.Interfaces;
using System.Reflection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ISTB.Framework.Executors.Storages.Implementations
{
    public class TargetMethodInfo
    {
        public MethodInfo MethodInfo { get; set; } = default!;
        public ICollection<TargetAttribute> TargetAttributes { get; set; } = default!;
    }

    public class TargetMethodStorage : ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }
        public Dictionary<UpdateType, ICollection<TargetMethodInfo>> MethodRouter { get; }

        public TargetMethodStorage(IEnumerable<Type> executorsTypes)
        {
            Methods = executorsTypes
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttributes<TargetAttribute>().Count() > 0)
                .Select(method => method.ReturnType == typeof(Task) ? method : 
                        throw new Exception($"Return type of method {method.Name} not Task"));

            MethodRouter = TargetMethodHelper.MethodsToMethodRoutes(Methods);
        }

        public MethodInfo? GetMethodInfoToExecute(Update update)
        {
            var methods = update.Type == UpdateType.Unknown ?
                MethodRouter[update.Type] :
                MethodRouter[update.Type].Concat(MethodRouter[UpdateType.Unknown]);

            return methods.FirstOrDefault(method =>
                method
                    .TargetAttributes
                    .Any(attr => attr.IsTarget(update))
            )?.MethodInfo;
        }
    }
}
