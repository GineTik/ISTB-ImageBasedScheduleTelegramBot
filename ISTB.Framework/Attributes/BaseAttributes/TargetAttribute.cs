using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class TargetAttribute : Attribute
    {
        public string UserState { get; set; } = default!;
        public abstract bool IsTarget(Update update);
    }
}
