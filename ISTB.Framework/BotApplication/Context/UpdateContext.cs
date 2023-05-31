using Telegram.Bot;
using Telegram.Bot.Types;

namespace ISTB.Framework.BotApplication.Context
{
    public class UpdateContext
    {
        public ITelegramBotClient Client { get; set; }
        public Update Update { get; set; }
        public long ChatId => Update.Message?.Chat?.Id ??
                              Update.CallbackQuery?.Message?.Chat?.Id ?? -1;
        public long TelegramUserId => Update.Message?.From?.Id ??
                              Update.CallbackQuery?.Message?.From?.Id ?? -1;
    }
}
