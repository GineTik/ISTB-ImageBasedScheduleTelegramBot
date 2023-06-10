using ISTB.Framework.BotApplication.Context;
using ISTB.Framework.BotApplication.TelegramBotClientInheritors;
using Telegram.Bot;
using Telegram.Bot.Types;

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


        public async Task<Message> SendTextAsync(string text, long? chatId = null)
        {
            chatId ??= UpdateContext.ChatId;
            return await UpdateContext.Client.SendTextMessageAsync(chatId, text);
        }
    }
}
