using ISTB.Framework.BotApplication.Context;

namespace ISTB.Framework.Attributes.ValidateInputDataAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ValidateInputDataAttribute : Attribute
    {
        public abstract Task<bool> ValidateAsync(UpdateContext context);
    }
}
