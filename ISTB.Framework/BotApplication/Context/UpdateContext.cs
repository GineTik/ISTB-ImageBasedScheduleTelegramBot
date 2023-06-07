using Telegram.Bot;
using Telegram.Bot.Types;

namespace ISTB.Framework.BotApplication.Context
{
    public class UpdateContext
    {
        public ITelegramBotClient Client { get; set; }
        public Update Update { get; set; }
        public long ChatId => Update.Message?.Chat?.Id ??
                              Update.CallbackQuery?.Message?.Chat?.Id ?? 
                              throw new InvalidDataException("Don't found ChatId");
        public long TelegramUserId => Update.Message?.From?.Id ??
                              Update.CallbackQuery?.Message?.From?.Id ?? 
                              throw new InvalidDataException("Don't found TelegramUserId");
    }
}
