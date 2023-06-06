using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.BotApplication.Delegates;

namespace ISTB.Framework.BotApplication.Middlewares
{
    public interface IMiddleware
    {
        public Task InvokeAsync(UpdateContext updateContext, NextDelegate next);
    }
}
