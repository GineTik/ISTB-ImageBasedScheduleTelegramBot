using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.TelegramBotApplication.Delegates;

namespace ISTB.Framework.TelegramBotApplication.Middlewares
{
    public interface IMiddleware
    {
        public Task InvokeAsync(UpdateContext updateContext, NextDelegate next);
    }
}
