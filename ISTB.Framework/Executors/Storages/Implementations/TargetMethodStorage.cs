using ISTB.Framework.Attributes.BaseAttributes;
using ISTB.Framework.Executors.Storages.Interfaces;
using System.Reflection;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Storages.Implementations
{
    public class TargetMethodStorage : ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        public TargetMethodStorage(IEnumerable<Type> executorsTypes)
        {
            Methods = executorsTypes
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttributes<TargetExecutorAttribute>().Count() > 0)
                .Select(method => method.ReturnType == typeof(Task) ? method : 
                        throw new Exception($"Return type of method {method.Name} not Task"));
        }

        public MethodInfo? GetMethodInfoByUpdate(Update update)
        {
            return Methods.FirstOrDefault(method =>
                method
                    .GetCustomAttributes<TargetExecutorAttribute>()
                    .Any(attr => attr.IsTarget(update))
            );
        }
    }
}
