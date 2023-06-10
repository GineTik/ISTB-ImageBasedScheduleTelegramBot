using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.Executors.Storages.Interfaces;
using System.Reflection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ISTB.Framework.Executors.Storages.Implementations
{
    public class TargetMethodStorage : ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }
        public Dictionary<UpdateType, List<MethodInfo>> MethodRouter { get; }

        public TargetMethodStorage(IEnumerable<Type> executorsTypes)
        {
            Methods = executorsTypes
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttributes<TargetAttribute>().Count() > 0)
                .Select(method => method.ReturnType == typeof(Task) ? method : 
                        throw new Exception($"Return type of method {method.Name} not Task"));

            var updateTypes = Enum.GetValues(typeof(UpdateType)).Cast<UpdateType>();
            MethodRouter = new (
                updateTypes.ToDictionary(updateType => updateType, updateType => new List<MethodInfo>())
            );

            foreach (MethodInfo method in Methods)
            {
                var updateTypeAttributes = method
                    .GetCustomAttributes<TargetAttribute>()
                    .SelectMany(attr => attr.GetType().GetCustomAttributes<TargetUpdateTypeAttribute>());

                if (updateTypeAttributes.Count() == 0)
                {
                    MethodRouter[UpdateType.Unknown].Add(method);
                    continue;
                }

                foreach (var attr in updateTypeAttributes)
                {
                    MethodRouter[attr.UpdateType].Add(method);
                }
            }
        }

        public MethodInfo? GetMethodInfoToExecute(Update update)
        {
            var methods = update.Type == UpdateType.Unknown ?
                MethodRouter[update.Type] :
                MethodRouter[update.Type].Concat(MethodRouter[UpdateType.Unknown]);

            return methods.FirstOrDefault(method =>
                method
                    .GetCustomAttributes<TargetAttribute>()
                    .Any(attr => attr.IsTarget(update))
            );
        }
    }
}
