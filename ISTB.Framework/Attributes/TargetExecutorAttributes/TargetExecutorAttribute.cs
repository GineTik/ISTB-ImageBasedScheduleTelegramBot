using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.TargetExecutorAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class TargetExecutorAttribute : Attribute
    {
        public abstract bool IsTarget(Message message);
    }
}
