using ISTB.Framework.Attributes.BaseAttributes;
using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.TargetExecutorAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TargetCallbacksDatasAttribute : TargetExecutorAttribute
    {
        public string[] CallbacksDatas { get; set; }

        public TargetCallbacksDatasAttribute(string callbacksDatas)
        {
            CallbacksDatas = callbacksDatas.Replace(" ", "").Split(",");
        }

        public override bool IsTarget(Update update)
        {
            if (update?.CallbackQuery?.Data is not { } data)
                return false;

            var targetData = data.Split(' ').First();
            return CallbacksDatas.Contains(targetData);
        }
    }
}
