using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Delegates;

namespace ISTB.Framework.Middlewares
{
    public interface IMiddleware
    {
        public Task InvokeAsync(UpdateContext updateContext, UpdateDelegate next);
    }
}
