using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.BotApplication.Delegates;
using ISTB.Framework.Executors.Middlewares;

namespace ISTB.Framework.BotApplication.Middlewares
{
    public class UpdateContextMiddleware : IMiddleware
    {
        private UpdateContextAccessor _accessor;

        public UpdateContextMiddleware(UpdateContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task InvokeAsync(UpdateContext updateContext, NextDelegate next)
        {
            _accessor.UpdateContext = updateContext;
            await next();
        }
    }
}
