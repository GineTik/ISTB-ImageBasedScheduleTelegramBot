﻿using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Delegates;

namespace ISTB.Framework.Middlewares
{
    public class UpdateContextMiddleware : IMiddleware
    {
        private UpdateContextAccessor _accessor;

        public UpdateContextMiddleware(UpdateContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task InvokeAsync(UpdateContext updateContext, UpdateDelegate next)
        {
            _accessor.UpdateContext = updateContext;
            await next(updateContext);
        }
    }
}