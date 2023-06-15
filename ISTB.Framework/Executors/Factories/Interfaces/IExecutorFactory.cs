namespace ISTB.Framework.Executors.Factories.Interfaces
{
    public interface IExecutorFactory
    {
        Executor CreateExecutor(Type type);
        T CreateExecutor<T>() where T : Executor;
    }
}
