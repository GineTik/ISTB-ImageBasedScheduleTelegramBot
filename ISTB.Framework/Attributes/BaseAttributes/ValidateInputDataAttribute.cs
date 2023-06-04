using ISTB.Framework.BotApplication.Context;

namespace ISTB.Framework.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ValidateInputDataAttribute : Attribute
    {
        public string ErrorMessage { get; set; }
        public abstract Task<bool> ValidateAsync(UpdateContext updateContext);
    }
}
