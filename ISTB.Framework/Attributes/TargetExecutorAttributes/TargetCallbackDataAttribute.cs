using ISTB.Framework.Attributes.BaseAttributes;
using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.TargetExecutorAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TargetCallbackDataAttribute : TargetExecutorAttribute
    {
        public string CallbackData { get; set; }

        public TargetCallbackDataAttribute(string callbackData)
        {
            CallbackData = callbackData;
        }

        public override bool IsTarget(Update update)
        {
            if (update?.CallbackQuery?.Data is not { } data)
                return false;

            return data == CallbackData;
        }
    }
}
