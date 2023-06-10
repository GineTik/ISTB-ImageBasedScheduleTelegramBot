using ISTB.Framework.BotApplication.Context;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ISTB.Framework.BotApplication.TelegramBotClientInheritors
{
    public class AdvancedTelegramBotClient : TelegramBotClient, IAdvancedTelegramBotClient
    {
        public UpdateContext UpdateContext { get; }

        public AdvancedTelegramBotClient(string token, UpdateContext updateContext, HttpClient? httpClient = null) : base(token, httpClient)
        {
            UpdateContext = updateContext;
        }

        public async Task<Message> SendTextResponseAsync(
            string text,
            int? messageThreadId = default,
            ParseMode? parseMode = default,
            IEnumerable<MessageEntity>? entities = default,
            bool? disableWebPagePreview = default,
            bool? disableNotification = default,
            bool? protectContent = default,
            int? replyToMessageId = default,
            bool? allowSendingWithoutReply = default,
            IReplyMarkup? replyMarkup = default,
            CancellationToken cancellationToken = default)
        {
            return await this.SendTextMessageAsync(
                UpdateContext.ChatId,
                text,
                messageThreadId,
                parseMode,
                entities,
                disableWebPagePreview,
                disableNotification,
                protectContent,
                replyToMessageId,
                allowSendingWithoutReply,
                replyMarkup,
                cancellationToken);
        }

        public async Task DeleteCallbackQueryMessage()
        {
            if (UpdateContext.Update.Type != UpdateType.CallbackQuery)
                throw new InvalidOperationException("UpdateType is not CallbackQuery");

            await AnswerCurrentCallbackQueryAsync();

            await this.DeleteMessageAsync(
                UpdateContext.ChatId,
                UpdateContext.Update.CallbackQuery.Message.MessageId);
        }

        public async Task DeleteButtonWithCallbacksDatas(Message message, params string[] callbacksDatas)
        {
            await DeleteButtonWithCallbacksDatas(UpdateContext.ChatId, message, callbacksDatas);
        }

        public async Task DeleteButtonWithCallbacksDatas(ChatId chatId, Message message, params string[] callbacksDatas)
        {
            ArgumentNullException.ThrowIfNull(chatId);
            ArgumentNullException.ThrowIfNull(message);
            ArgumentNullException.ThrowIfNull(message.ReplyMarkup?.InlineKeyboard);

            var messageKeyboard = message.ReplyMarkup?.InlineKeyboard;
            var messageId = message.MessageId;
            var newKeyboard = messageKeyboard.Select(row =>
                row.SkipWhile(button => callbacksDatas.Contains(button.CallbackData))
            );

            await this.EditMessageReplyMarkupAsync(
                chatId,
                messageId,
                replyMarkup: new InlineKeyboardMarkup(newKeyboard)
            );
        }

        public async Task AnswerCurrentCallbackQueryAsync()
        {
            if (UpdateContext.Update.Type != UpdateType.CallbackQuery)
                throw new InvalidOperationException("UpdateType is not CallbackQuery");

            await this.AnswerCallbackQueryAsync(
               UpdateContext.Update.CallbackQuery.Id
            );
        }
    }
}
