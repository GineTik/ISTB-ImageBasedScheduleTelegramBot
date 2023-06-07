using ISTB.Framework.BotApplication.Context;
using System.Reflection;
using Telegram.Bot.Types;

namespace ISTB.Framework.Executors.Storages.Interfaces
{
    public interface ITargetMethodStorage
    {
        public IEnumerable<MethodInfo> Methods { get; }

        MethodInfo? GetMethodInfoToExecute(Update update);
    }
}
