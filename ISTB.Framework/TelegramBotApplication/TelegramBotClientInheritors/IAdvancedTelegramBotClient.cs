using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.Framework.TelegramBotApplication.TelegramBotClientInheritors
{
    public interface IAdvancedTelegramBotClient : ITelegramBotClient
    {
        Task<Message> SendTextResponseAsync(string text,
            int? messageThreadId = default,
            ParseMode? parseMode = default,
            IEnumerable<MessageEntity>? entities = default,
            bool? disableWebPagePreview = default,
            bool? disableNotification = default,
            bool? protectContent = default,
            int? replyToMessageId = default,
            bool? allowSendingWithoutReply = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default);
        Task DeleteCallbackQueryMessage();
        Task DeleteButtonWithCallbacksDatas(Message message, params string[] callbacksDatas);
        Task DeleteButtonWithCallbacksDatas(ChatId chatId, Message message, params string[] callbacksDatas);
        Task AnswerCurrentCallbackQueryAsync();
    }
}
