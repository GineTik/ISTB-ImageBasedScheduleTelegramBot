using ISTB.Framework.TelegramBotApplication.Context;

namespace ISTB.Framework.Attributes.BaseAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class ValidateInputDataAttribute : Attribute
    {
        public string ErrorMessage { get; set; } = default!;
        public abstract Task<bool> ValidateAsync(UpdateContext updateContext, IServiceProvider provider);
    }
}
