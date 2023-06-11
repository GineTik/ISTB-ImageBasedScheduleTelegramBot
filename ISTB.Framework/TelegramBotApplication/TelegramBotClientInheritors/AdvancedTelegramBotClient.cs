using ISTB.Framework.TelegramBotApplication.Context;
using Telegram.Bot;

namespace ISTB.Framework.TelegramBotApplication.TelegramBotClientInheritors
{
    public class AdvancedTelegramBotClient : TelegramBotClient, IAdvancedTelegramBotClient
    {
        public UpdateContext UpdateContext { get; }

        public AdvancedTelegramBotClient(string token, UpdateContext updateContext, HttpClient? httpClient = null) : base(token, httpClient)
        {
            UpdateContext = updateContext;
        }
    }
}
