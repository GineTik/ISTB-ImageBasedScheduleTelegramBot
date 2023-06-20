using ISTB.Framework.TelegramBotApplication.Context;
using ISTB.Framework.TelegramBotApplication.AdvancedBotClient;

namespace ISTB.Framework.Executors
{
    public abstract class Executor
    {
        public UpdateContext UpdateContext
        {
            get => _updateContext ?? throw new NullReferenceException(nameof(UpdateContext) + ", maybe you created ececutor not correct");
            set => _updateContext = value ?? throw new ArgumentNullException(nameof(value));
        }
        public IAdvancedTelegramBotClient Client => UpdateContext?.Client;

        private UpdateContext _updateContext;
    }
}
