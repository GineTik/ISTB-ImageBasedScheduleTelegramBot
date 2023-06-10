using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.Executors;

namespace ISTB.Framework.Executors.Factories.Interfaces
{
    public interface IExecutorFactory
    {
        Executor CreateExecutor(Type type);
    }
}
