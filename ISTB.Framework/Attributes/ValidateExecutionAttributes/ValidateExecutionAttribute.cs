using Telegram.Bot.Types;

namespace ISTB.Framework.Attributes.ValidateExecutionAttributes
{
    public abstract class ValidateExecutionAttribute : Attribute
    {
        public abstract bool ValidateExecution(Message message);
    }
}
