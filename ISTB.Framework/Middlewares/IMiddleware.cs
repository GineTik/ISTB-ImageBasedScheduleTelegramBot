using ISTB.Framework.Delegates;
using ISTB.Framework.Executors.Context;

namespace ISTB.Framework.Middlewares
{
    public interface IMiddleware
    {
        public Task InvokeAsync(UpdateContext updateContext, UpdateDelegate next);
    }
}
