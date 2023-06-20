using ISTB.Framework.TelegramBotApplication.Context;
using System.Reflection;

namespace ISTB.Framework.Executors.Routing.Storages.TargetMethod
{
    public interface ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        Task<MethodInfo?> GetMethodInfoToExecuteAsync(UpdateContext updateContext);
    }
}
