using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class TargetAttribute : Attribute
    {
        public abstract bool IsTarget(Update update);
    }
}
