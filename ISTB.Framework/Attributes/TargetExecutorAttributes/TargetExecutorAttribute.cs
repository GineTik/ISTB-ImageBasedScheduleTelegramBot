using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.TargetExecutorAttributes
{
    public abstract class TargetExecutorAttribute : Attribute
    {
        public abstract bool IsTarget(Message message);
    }
}
