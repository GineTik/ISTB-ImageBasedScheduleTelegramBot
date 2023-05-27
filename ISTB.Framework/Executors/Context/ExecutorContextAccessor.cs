namespace ISTB.Framework.Executors.Context
{
    public class ExecutorContextAccessor
    {
        private static readonly AsyncLocal<ExecutorContext> _executorContextCurrent = new AsyncLocal<ExecutorContext>();

        public ExecutorContext? ExecutorContext
        {
            get
            {
                return _executorContextCurrent.Value;
            }
            set
            {
                if (value == null)
                    return;

                if (_executorContextCurrent.Value != null)
                    return;

                _executorContextCurrent.Value = value;
            }
        }

    }
}
