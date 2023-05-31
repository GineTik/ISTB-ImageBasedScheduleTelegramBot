using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Executors;

namespace ISTB.Framework.Factories.Interfaces
{
    public interface IExecutorFactory
    {
        Executor CreateExecutor(Type type, UpdateContext context);
    }
}
