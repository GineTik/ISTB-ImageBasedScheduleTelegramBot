using ISTB.Framework.BotApplication.Context;

namespace ISTB.Framework.BotApplication.Extensions.Middlwares
{
    public static class CatchExceptionExtension
    {
        public static BotApplication UseCatchException(this BotApplication app, Action<UpdateContext, Exception> action)
        {
            return app.UseCatchException<Exception>(action);
        }

        public static BotApplication UseCatchException<TException>(this BotApplication app, Action<UpdateContext, TException> action)
            where TException : Exception
        {
            return app.Use(async (updateContext, next) =>
            {
                try
                {
                    await next();
                }
                catch (TException ex)
                {
                    action.Invoke(updateContext, ex);
                }
            });
        }
    }
}
