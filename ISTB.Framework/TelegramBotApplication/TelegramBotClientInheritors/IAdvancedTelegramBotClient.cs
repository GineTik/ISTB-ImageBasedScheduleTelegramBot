using ISTB.Framework.TelegramBotApplication.Context;
using Telegram.Bot;

namespace ISTB.Framework.TelegramBotApplication.TelegramBotClientInheritors
{
    public interface IAdvancedTelegramBotClient : ITelegramBotClient
    {
        public UpdateContext UpdateContext { get; }
    }
}
