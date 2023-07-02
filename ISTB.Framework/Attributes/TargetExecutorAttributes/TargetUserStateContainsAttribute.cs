using ISTB.Framework.Attributes.BaseAttributes;
using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.TargetExecutorAttributes
{
    public class TargetUserStateContainsAttribute : TargetAttribute
    {
        public TargetUserStateContainsAttribute(string userStates)
        {
            UserStates = userStates;
        }

        public override bool IsTarget(Update update)
        {
            return true;
        }
    }
}
