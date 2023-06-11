using ISTB.Framework.Attributes.ParametersParse;
using ISTB.Framework.Attributes.TargetExecutorAttributes;
using ISTB.Framework.TelegramBotApplication.Storages.Interfaces;
using ISTB.Framework.Executors;
using Telegram.Bot;
using ISTB.Framework.TelegramBotApplication.Extensions.AdvancedTelegramBotClient;
using ISTB.BusinessLogic.DTOs.Schedule;
using ISTB.Framework.MessagePresets.Extensions.AdvancedTelegramBotClient;
using ISTB.TelegramBot.Enum.Buttons;
using Telegram.Bot.Types.ReplyMarkups;
using ISTB.DataAccess.Entities;
using ISTB.Framework.TelegramBotApplication.Builders;

namespace ISTB.TelegramBot.Executors
{
    public class BasicExecutor : Executor
    {
        private readonly ICommandStorage _commandStorage;

        public BasicExecutor(ICommandStorage commandStorage)
        {
            _commandStorage = commandStorage;
        }

        [TargetCommands("start")]
        public async Task StartCommand()
        {
            await Client.SendTextResponseAsync("Success");
            await Client.SetMyCommandsAsync(_commandStorage.Commands);
        }

        [TargetCallbacksDatas("confirm_act")]
        [ParametersSeparator(" | ")]
        public async Task ConfirmYourAct(string targetCallbackData, string data)
        {
            await Client.AnswerCallbackQueryAsync();
            await Client.SendTextResponseAsync(
                "Ви впевнені?",
                replyMarkup: new InlineKeyboardBuilder()
                    .CallbackButton("Так", $"{targetCallbackData} {data}")
                    .CallbackButton("Ні", "act_not_confirm").EndRow()
                    .Build()
            );
        }

        [TargetCallbacksDatas("act_not_confirm")]
        public async Task FailtureRemoveSchedule()
        {
            await Client.AnswerCallbackQueryAsync();
            await Client.DeleteCallbackQueryMessageAsync();
        }
    }
}
