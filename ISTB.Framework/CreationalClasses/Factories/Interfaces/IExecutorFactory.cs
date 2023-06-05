using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Executors;

namespace ISTB.Framework.CreationalClasses.Factories.Interfaces
{
    public interface IExecutorFactory
    {
        Executor CreateExecutor(Type type, UpdateContext context);
    }
}
