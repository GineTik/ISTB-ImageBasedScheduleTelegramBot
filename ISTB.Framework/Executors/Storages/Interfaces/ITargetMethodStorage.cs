using System.Reflection;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Storages.Interfaces
{
    public interface ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        MethodInfo? GetMethodInfoByUpdate(Update update);
    }
}
