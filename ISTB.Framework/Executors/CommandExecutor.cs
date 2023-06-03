using ISTB.Framework.Parsers;
using System.Text.RegularExpressions;

namespace ISTB.Framework.Executors
{
    public abstract class CommandExecutor<TParameters> : Executor
        where TParameters : class, new()
    {
        private TParameters? _parameters;
        public TParameters? Parameters
        {
            get
            {
                if (_parameters != null)
                    return _parameters;

                var result = CommandParametersParser.Parse<TParameters>(UpdateContext.Update.Message?.Text ?? "");
                return _parameters ??= (result.Value as TParameters);
            }
        }
    }
}
 